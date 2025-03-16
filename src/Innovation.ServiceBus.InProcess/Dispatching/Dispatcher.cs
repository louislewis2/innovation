namespace Innovation.ServiceBus.InProcess.Dispatching
{
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.DependencyInjection;

    using Settings;
    using Api.Core;
    using Validators;
    using Exceptions;
    using Api.Querying;
    using Api.Reactions;
    using Api.Messaging;
    using Api.Commanding;
    using Api.Validation;
    using Api.Dispatching;
    using Api.Interceptors;
    using Api.CommandHelpers;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// This is the implementation of the dispatcher for the in process service bus.
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        #region Fields

        private readonly ILogger logger;
        private readonly InnovationOptions innovationOptions;
        private readonly InnovationRuntime innovationRuntime;

        private static readonly ICommandResult commandResultStatic = new CommandResult();

        private IServiceScope serviceScope;

        #endregion Fields

        #region Constructor

        public Dispatcher(
            ILogger<Dispatcher> logger,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<InnovationOptions> innovationOptionsOptions,
            InnovationRuntime innovationRuntime)
        {
            this.logger = logger;
            this.innovationOptions = innovationOptionsOptions.Value;
            this.innovationRuntime = innovationRuntime;

            this.serviceScope = serviceScopeFactory.CreateScope();
        }

        #endregion Constructor

        #region Properties

        public IDispatcherContext Context { get; private set; }
        public string CorrelationId { get; private set; } = Guid.NewGuid().ToString();

        #endregion Properties

        #region Methods

        public void SetCorrelationId([DisallowNull] string correlationId)
        {
            if (!string.IsNullOrWhiteSpace(value: correlationId))
            {
                this.CorrelationId = correlationId;
            }
        }

        public void SetContext([DisallowNull] IDispatcherContext dispatcherContext)
        {
            this.Context = dispatcherContext ?? throw new ArgumentNullException(paramName: nameof(dispatcherContext));
            this.Context.SetCorrelationId(correlationId: this.CorrelationId);
        }

        public async ValueTask<ICommandResult> Command<TCommand>([DisallowNull] TCommand command, bool suppressExceptions = true) where TCommand : ICommand
        {
            /* Returning at this point with static CommandResult or even null

            | Method  | Mean     | Error    | StdDev   | Gen0   | Allocated |
            |-------- |---------:|---------:|---------:|-------:|----------:|
            | Command | 48.29 ns | 0.339 ns | 0.283 ns | 0.0038 |      40 B |
             */

            var stopWatch = ValueStopwatch.StartNew();

            try
            {
                /* Returning at this point with static CommandResult or even null

                | Method  | Mean     | Error    | StdDev   | Gen0   | Allocated |
                |-------- |---------:|---------:|---------:|-------:|----------:|
                | Command | 58.64 ns | 1.028 ns | 0.912 ns | 0.0038 |      40 B |
                 */

                var commandType = command.GetType();

                if (command == null)
                {
                    DispatcherLogging.CommandParameterNull(this.logger);
                    throw new ArgumentNullException(paramName: nameof(command));
                }

                /* Returning at this point with static CommandResult or even null

                | Method  | Mean     | Error    | StdDev   | Gen0   | Allocated |
                |-------- |---------:|---------:|---------:|-------:|----------:|
                | Command | 59.55 ns | 0.178 ns | 0.158 ns | 0.0038 |      40 B |
                 */

                DispatcherLogging.EnteredCommandDispatcher(
                    logger: this.logger, 
                    correlationId: this.CorrelationId, 
                    commandName: command.EventName, 
                    commandType: commandType);

                DispatcherLogging.CommandDetail(logger: this.logger, command: command);

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error    | StdDev   | Gen0   | Allocated |
                |-------- |---------:|---------:|---------:|-------:|----------:|
                | Command | 72.37 ns | 1.088 ns | 0.964 ns | 0.0038 |      40 B |
                 */

                var commandBitsForCommandType = this.innovationRuntime.GetCommandBits(commandType: commandType);

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error    | StdDev   | Gen0   | Allocated |
                |-------- |---------:|---------:|---------:|-------:|----------:|
                | Command | 89.49 ns | 0.663 ns | 0.588 ns | 0.0038 |      40 B |
                 */

                if ((commandBitsForCommandType & (1 << (int)CommandBitTypes.CommandHandlerRegistered)) <= 0)
                {
                    // TODO: Ensure the runtime logs this as an error
                    // The runtime should check and ensure all commands have handlers registered
                    DispatcherLogging.CommandHandlerNotFound(logger: this.logger, eventName: command.EventName, commandType: commandType);

                    throw new CommandHandlerNotFoundException(command: command);
                }

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error    | StdDev   | Gen0   | Allocated |
                |-------- |---------:|---------:|---------:|-------:|----------:|
                | Command | 90.50 ns | 1.450 ns | 1.356 ns | 0.0038 |      40 B |
                 */

                if (this.Context == null)
                {
                    DispatcherLogging.ContextNotSet(
                        logger: this.logger,
                        commandName: command.EventName,
                        correlationId: this.CorrelationId,
                        commandType: commandType);
                }
                else
                {

                    if ((commandBitsForCommandType & (1 << (int)CommandBitTypes.ContextAware)) != 0)
                    {
                        var contextAwareCommand = command as IContextAware;
                        contextAwareCommand.SetContext(dispatcherContext: this.Context);
                    }
                }

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error    | StdDev   | Gen0   | Allocated |
                |-------- |---------:|---------:|---------:|-------:|----------:|
                | Command | 91.99 ns | 0.883 ns | 0.738 ns | 0.0038 |      40 B |
                 */

                var commandHandler = this.serviceScope.ServiceProvider.GetService<ICommandHandler<TCommand>>();

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 114.2 ns | 1.08 ns | 0.90 ns | 0.0060 |      64 B | <----------------------- 24 Bytes Increase
                 */

                // Just because there is a command handler has been registered, it does not mean we can get an instance.
                // If it is missing dependencies for example, it cannot be instantiated, though this normally throws an error
                // lets not make assumptions and be safe
                if (commandHandler == null)
                {
                    DispatcherLogging.CommandHandlerNotFound(logger: this.logger, eventName: command.EventName, commandType: commandType);

                    throw new CommandHandlerNotFoundException(command: command);
                }

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 111.6 ns | 1.32 ns | 1.24 ns | 0.0061 |      64 B |
                 */

                DispatcherLogging.CommandHandlerFound(logger: this.logger, commandHandler.GetType());

                if ((commandBitsForCommandType & (1 << (int)CommandBitTypes.CorrelationIdAware)) != 0)
                {
                    var correlationAwareCommand = command as ICorrelationAware;
                    correlationAwareCommand.CorrelationId = this.CorrelationId;
                }

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 111.6 ns | 1.32 ns | 1.24 ns | 0.0061 |      64 B |
                 */

                // If the command has reactors registered, process them
                if ((commandBitsForCommandType & (1 << (int)CommandBitTypes.CommandReactor)) != 0)
                {
                    var commandReactors = this.ResolveCommandReactors<TCommand>();

                    if (commandReactors != null)
                    {
                        DispatcherLogging.CommandReactorsFound(logger: this.logger, commandReactorCount: commandReactors.Length);
                    }

                    if (commandReactors.Length > 0)
                    {
                        _ = Task.Run(() =>
                        {
                            DispatcherLogging.NotifyingCommandReactors(logger: this.logger);

                            this.NotifyCommandReactors(commandReactors: commandReactors, command: command);
                        });
                    }
                }

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 116.1 ns | 1.45 ns | 1.29 ns | 0.0061 |      64 B |
                 */

                // If the command has interceptor registered, process them
                if ((commandBitsForCommandType & (1 << (int)CommandBitTypes.CommandInterceptor)) != 0)
                {
                    var commandInterceptors = this.ResolveCommandInterceptors<TCommand>();

                    if (commandInterceptors != null && commandInterceptors.Length > 0)
                    {
                        DispatcherLogging.CommandInterceptorsFound(logger: this.logger, commandInterceptorCount: commandInterceptors.Length);

                        foreach (var commandInterceptor in commandInterceptors)
                        {
                            // Using this nested try / catch block to avoid interceptor exceptions breaking the pipeline
                            try
                            {
                                DispatcherLogging.CommandInterceptorGoingToRun(logger: this.logger, commandInterceptorType: commandInterceptor.GetType());

                                await commandInterceptor.Intercept(command: command);
                            }
                            catch (Exception ex)
                            {
                                DispatcherLogging.CommandInterceptorRaisedException(logger: this.logger, commandInterceptorType: commandInterceptor.GetType());
                                this.logger.LogError(exception: ex, message: ex.GetInnerMostMessage());
                            }
                        }
                    }
                }

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 114.7 ns | 2.26 ns | 2.12 ns | 0.0061 |      64 B |
                 */

                ICommandResult commandResult = null;
                IValidationResult validationResult = null;

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 109.3 ns | 2.18 ns | 2.42 ns | 0.0061 |      64 B |
                 */

                if ((commandBitsForCommandType & (1 << (int)CommandBitTypes.IsValidationEnabled)) != 0)
                {
                    /* Returning at this point with static CommandResult or even null
                     * 
                    | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                    |-------- |---------:|--------:|--------:|-------:|----------:|
                    | Command | 115.1 ns | 1.91 ns | 1.60 ns | 0.0083 |      88 B |<---------------------- 24 Bytes allocated here
                     */

                    var dataAnnotationsValidator = new DataAnnotationsValidator(serviceProvider: this.serviceScope.ServiceProvider);

                    /* Returning at this point with static CommandResult or even null
                     * 
                    | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                    |-------- |---------:|--------:|--------:|-------:|----------:|
                    | Command | 118.0 ns | 2.36 ns | 2.21 ns | 0.0083 |      88 B |
                     */

                    var validatorResult = await dataAnnotationsValidator.TryValidateObjectRecursive(target: command);

                    /* Returning at this point with static CommandResult or even null
                     * 
                    | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                    |-------- |---------:|--------:|--------:|-------:|----------:|
                    | Command | 222.0 ns | 1.86 ns | 1.55 ns | 0.0167 |     176 B |<---------------------- 88 Bytes allocated here
                     */

                    DispatcherLogging.CommandInitialValidationResult(logger: this.logger, eventName: command.EventName, isValid: validatorResult.isValid);

                    if (!validatorResult.isValid && validatorResult.Errors?.Count > 0)
                    {
                        commandResult = new CommandResult(errors: validatorResult.Errors);
                    }

                    /* Returning at this point with static CommandResult or even null
                     * 
                    | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                    |-------- |---------:|--------:|--------:|-------:|----------:|
                    | Command | 226.5 ns | 2.49 ns | 2.08 ns | 0.0167 |     176 B |
                     */

                    if (validatorResult.isValid)
                    {
                        // If the command has validators registered, process them
                        if ((commandBitsForCommandType & (1 << (int)CommandBitTypes.CommandValidator)) != 0)
                        {
                            var commandValidators = this.ResolveCommandValidators<TCommand>();

                            if (commandValidators != null)
                            {
                                DispatcherLogging.CommandValidatorsFound(logger: this.logger, commandValidatorCount: commandValidators.Length);
                            }

                            if (commandValidators != null && commandValidators.Length > 0)
                            {
                                foreach (var commandValidator in commandValidators)
                                {
                                    var intermediateValidationResult = await commandValidator.Validate(command: command);

                                    if (!intermediateValidationResult.Success)
                                    {
                                        validationResult = intermediateValidationResult;

                                        break;
                                    }
                                }
                            }
                        }

                        /* Returning at this point with static CommandResult or even null
                         * 
                        | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                        |-------- |---------:|--------:|--------:|-------:|----------:|
                        | Command | 220.3 ns | 1.28 ns | 1.07 ns | 0.0167 |     176 B |
                         */
                    }
                }

                var finalResult = validationResult ?? (commandResult == null ? await commandHandler.Handle(command: command) : commandResult.Success ? await commandHandler.Handle(command: command) : commandResult);

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 239.3 ns | 2.00 ns | 1.67 ns | 0.0167 |     176 B |
                 */

                // If the command has result reactors registered, process them
                if ((commandBitsForCommandType & (1 << (int)CommandBitTypes.CommandResultReactor)) != 0)
                {
                    var commandResultReactors = this.ResolveCommandResultReactors<TCommand>();

                    if (commandResultReactors != null)
                    {
                        DispatcherLogging.CommandResultReactorsFound(logger: this.logger, commandResultReactorCount: commandResultReactors.Length);
                    }

                    if (commandResultReactors.Length > 0)
                    {
                        _ = Task.Run(() =>
                        {
                            DispatcherLogging.NotifyingCommandResultReactors(logger: this.logger);

                            this.NotifyCommandResultReactors(commandResultReactors: commandResultReactors, command: command, commandResult: finalResult);
                        });
                    }
                }

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 227.7 ns | 0.79 ns | 0.66 ns | 0.0167 |     176 B |
                 */

                DispatcherLogging.ReturningFromDispatcher(this.logger, finalResult.Success);

                if (this.innovationRuntime.HasAuditStoreRegistered)
                {
                    var auditStore = this.serviceScope.ServiceProvider.GetService<IAuditStore>();

                    /* Returning at this point with static CommandResult or even null
                     * 
                    | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                    |-------- |---------:|--------:|--------:|-------:|----------:|
                    | Command | 251.7 ns | 1.39 ns | 1.23 ns | 0.0167 |     176 B |
                     */

                    if (auditStore != null)
                    {
                        DispatcherLogging.AuditStoreFound(logger: this.logger, auditStoreType: auditStore.GetType());
                        await auditStore.Log(
                            auditContext: new AuditContext(correlationId: this.CorrelationId, runtimeMilliSeconds: (long)stopWatch.GetElapsedTime().TotalMilliseconds),
                            command: command,
                            commandResult: finalResult);

                        /* Returning at this point with static CommandResult or even null
                         * 
                        | Method  | Mean     | Error   | StdDev   | Gen0   | Allocated |
                        |-------- |---------:|--------:|---------:|-------:|----------:|
                        | Command | 332.1 ns | 6.52 ns | 10.15 ns | 0.0196 |     208 B |<---------------------- 32 Bytes allocated here
                         */
                    }
                }

                /* Returning at this point with static CommandResult or even null
                 * 
                | Method  | Mean     | Error   | StdDev  | Gen0   | Allocated |
                |-------- |---------:|--------:|--------:|-------:|----------:|
                | Command | 330.7 ns | 6.44 ns | 6.61 ns | 0.0196 |     208 B |
                 */

                return finalResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(exception: ex, message: ex.GetInnerMostMessage());

                if (suppressExceptions)
                {
                    return this.CreateFromException(ex);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Message<TMessage>([DisallowNull] TMessage message) where TMessage : IMessage
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            this.logger.LogDebug(2, "Entered Message Dispatcher. {correlationId}", this.CorrelationId);

            var auditStore = this.serviceScope.ServiceProvider.GetService<IAuditStore>();

            if (auditStore != null)
            {
                this.logger.LogDebug("Audit Store Found - {AuditStoreType}", auditStore.GetType());
            }

            var handlers = this.ResolveMessageHandlers<TMessage>();

            if (auditStore != null)
            {
                await auditStore.Log(auditContext: new AuditContext(correlationId: this.CorrelationId, runtimeMilliSeconds: stopWatch.ElapsedMilliseconds), message: message);
            }

            foreach (var handler in handlers)
            {
                await Task.Run(() => handler.Handle(message));
            }
        }

        public async Task MessageFor<TMessage>([DisallowNull] TMessage message, [DisallowNull] params string[] addresses) where TMessage : IMessage
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            this.logger.LogDebug(2, "Entered MessageFor Dispatcher. {correlationId}", this.CorrelationId);

            var auditStore = this.serviceScope.ServiceProvider.GetService<IAuditStore>();

            if (auditStore != null)
            {
                this.logger.LogDebug("Audit Store Found - {AuditStoreType}", auditStore.GetType());
            }

            var handlers = this.ResolveMessageHandlers<TMessage>();

            var addressableHandlers = handlers.OfType<IAddressable>().Where(x => x.Handles.Intersect(addresses).Any()).Cast<IMessageHandler<TMessage>>();

            if (addressableHandlers == null || !addressableHandlers.Any())
            {
                this.logger.LogError("Addressable Message Handlers Not Found - {MessageName} - {MessageType} - {Addresses}", message.EventName, message.GetType(), addresses);
                return;
            }

            if (auditStore != null)
            {
                await auditStore.Log(auditContext: new AuditContext(correlationId: this.CorrelationId, runtimeMilliSeconds: stopWatch.ElapsedMilliseconds), message: message);
            }

            foreach (var handler in addressableHandlers)
            {
                await Task.Run(() => handler.Handle(message));
            }
        }

        public async Task<TQueryResult> Query<TQuery, TQueryResult>([DisallowNull] TQuery query) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                var auditStore = this.serviceScope.ServiceProvider.GetService<IAuditStore>();

                if (auditStore != null)
                {
                    this.logger.LogDebug("Audit Store Found - {AuditStoreType}", auditStore.GetType());
                }

                this.logger.LogDebug(3, "Entered Query Dispatcher. {correlationId}", this.CorrelationId);

                var queryHandler = this.Resolve<TQuery, TQueryResult>();

                if (queryHandler == null)
                {
                    this.logger.LogError("Query Handler Not Found - {QueryName} - {QueryType} - {ResultType}", query.EventName, query.GetType(), typeof(TQueryResult));
                    throw new QueryHandlerNotFoundException(query);
                }

                if (query is IContextAware contextAwareQuery)
                {
                    if (this.Context == null)
                    {
                        this.logger.LogWarning(1, "Query {QueryName} Is Context Aware, Context Was Not Set.{correlationId} - {CommandType}", query.EventName, this.CorrelationId, query.GetType());
                    }
                    else
                    {
                        contextAwareQuery.SetContext(dispatcherContext: this.Context);
                    }
                }

                if (queryHandler is ICorrelationAware correlationAwareQuery)
                {
                    correlationAwareQuery.CorrelationId = this.CorrelationId;
                }

                this.logger.LogDebug(3, "Found Handler - {HandlerType} - {ResultType}", query.GetType(), typeof(TQueryResult));

                this.logger.LogDebug(3, "Calling QueryHandler");

                if (auditStore != null)
                {
                    await auditStore.Log(auditContext: new AuditContext(correlationId: this.CorrelationId, runtimeMilliSeconds: stopWatch.ElapsedMilliseconds), query: query);
                }

                return await queryHandler.Handle(query);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);

                throw;
            }
        }

        public async Task<TQueryResult> QueryFor<TQuery, TQueryResult>([DisallowNull] TQuery query, [DisallowNull] params string[] addresses) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                this.logger.LogDebug(3, "Entered Query Dispatcher. {correlationId}", this.CorrelationId);

                var auditStore = this.serviceScope.ServiceProvider.GetService<IAuditStore>();

                if (auditStore != null)
                {
                    this.logger.LogDebug("Audit Store Found - {AuditStoreType}", auditStore.GetType());
                }

                var queryHandlers = this.ResolveAll<TQuery, TQueryResult>();

                var addressableHandlers = queryHandlers.OfType<IAddressable>().Where(x => x.Handles.Intersect(addresses).Any()).Cast<IQueryHandler<TQuery, TQueryResult>>();

                if (addressableHandlers == null || !addressableHandlers.Any())
                {
                    this.logger.LogError("Query Handlers Not Found - {QueryName} - {QueryType} - {ResultType} - {Addresses}", query.EventName, query.GetType(), typeof(TQueryResult), addresses);
                    throw new QueryHandlerNotFoundException(query);
                }

                var queryHandler = addressableHandlers.First();

                this.logger.LogDebug(3, "Found Handler - {HandlerType} - {ResultType}", query.GetType(), typeof(TQueryResult));

                this.logger.LogDebug(3, "Calling QueryHandler");

                if (auditStore != null)
                {
                    await auditStore.Log(auditContext: new AuditContext(correlationId: this.CorrelationId, runtimeMilliSeconds: stopWatch.ElapsedMilliseconds), query: query);
                }

                return await queryHandler.Handle(query);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);

                throw;
            }
        }

        #endregion Methods

        #region Private Methods

        private void NotifyCommandReactors<TCommand>([DisallowNull] ICommandReactor<TCommand>[] commandReactors, [DisallowNull] TCommand command) where TCommand : ICommand
        {
            this.logger.LogDebug(4, "Entered NotifyCommandReactors. {correlationId}", this.CorrelationId);

            foreach (var reactor in commandReactors)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await reactor.React(command);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogDebug($"The Command Reactor: {reactor.GetType()} Raised An Exception");
                        this.logger.LogError(ex.Message, ex);
                    }
                });
            }
        }

        private void NotifyCommandResultReactors<TCommand, TCommandResult>([DisallowNull] ICommandResultReactor<TCommand>[] commandResultReactors, [DisallowNull] TCommand command, [DisallowNull] TCommandResult commandResult)
            where TCommand : ICommand
            where TCommandResult : ICommandResult
        {
            this.logger.LogDebug(5, "Entered NotifyCommandResultReactors. {correlationId}", this.CorrelationId);


            foreach (var reactor in commandResultReactors)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await reactor.React(commandResult, command);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(5, "Error Occurred NotifyCommandResultReactors. {correlationId} {exceptionMessage }", this.CorrelationId, ex.Message);
                    }
                });
            }
        }

        private ICommandResult CreateFromException([DisallowNull] Exception ex)
        {
            var exceptionResult = new CommandExceptionResult(message: ex.Message, exception: ex);

            return exceptionResult;
        }

        private ICommandReactor<TCommand>[] ResolveCommandReactors<TCommand>() where TCommand : ICommand
        {
            var commandReactors = this.serviceScope.ServiceProvider.GetServices<ICommandReactor<TCommand>>();

            return commandReactors.ToArray();
        }

        private ICommandInterceptor<TCommand>[] ResolveCommandInterceptors<TCommand>() where TCommand : ICommand
        {
            var commandInterceptors = this.serviceScope.ServiceProvider.GetServices<ICommandInterceptor<TCommand>>();

            return commandInterceptors.ToArray();
        }

        private ICommandResultReactor<TCommand>[] ResolveCommandResultReactors<TCommand>() where TCommand : ICommand
        {
            var commandResultReactors = this.serviceScope.ServiceProvider.GetServices<ICommandResultReactor<TCommand>>();

            return commandResultReactors.ToArray();
        }

        private IMessageHandler<TMessage>[] ResolveMessageHandlers<TMessage>() where TMessage : IMessage
        {
            var commandHandlers = this.serviceScope.ServiceProvider.GetServices<IMessageHandler<TMessage>>();

            return commandHandlers.ToArray();
        }

        private IValidator<TCommand>[] ResolveCommandValidators<TCommand>() where TCommand : ICommand
        {
            var commandValidators = this.serviceScope.ServiceProvider.GetServices<IValidator<TCommand>>();

            return commandValidators.ToArray();
        }

        private IQueryHandler<TQuery, TQueryResult> Resolve<TQuery, TQueryResult>()
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            var queryHandler = this.serviceScope.ServiceProvider.GetService<IQueryHandler<TQuery, TQueryResult>>();

            return queryHandler;
        }

        private IQueryHandler<TQuery, TQueryResult>[] ResolveAll<TQuery, TQueryResult>()
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            var queryHandler = this.serviceScope.ServiceProvider.GetServices<IQueryHandler<TQuery, TQueryResult>>();

            return queryHandler.ToArray();
        }

        #endregion Private Methods
    }
}