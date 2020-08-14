namespace Innovation.ServiceBus.InProcess.Dispatching
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Extensions.DependencyInjection;

    using Exceptions;
    using Validators;
    using Api.Querying;
    using Api.Reactions;
    using Api.Messaging;
    using Api.Commanding;
    using Api.Validation;
    using Api.Dispatching;
    using Api.Interceptors;
    using Api.CommandHelpers;
    using System.Diagnostics;

    /// <summary>
    /// This is the implementation of the dispatcher for the in process service bus.
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        #region Fields

        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IServiceScopeFactory serviceScopeFactory;

        #endregion Fields

        #region Constructor

        public Dispatcher(
            ILogger<Dispatcher> logger,
            IServiceProvider serviceProvider,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        #endregion Constructor

        #region Properties

        public IDispatcherContext Context { get; private set; }
        public string CorrelationId { get; private set; } = Guid.NewGuid().ToString();

        #endregion Properties

        #region Methods

        public void SetCorrelationId(string correlationId)
        {
            this.CorrelationId = correlationId;
        }

        public void SetContext(IDispatcherContext dispatcherContext)
        {
            this.Context = dispatcherContext ?? throw new ArgumentNullException(paramName: nameof(dispatcherContext));
            this.Context.SetCorrelationId(correlationId: this.CorrelationId);
        }

        public bool CanCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            try
            {
                var commandHandler = this.Resolve<TCommand>();

                return commandHandler != null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public bool CanMessage<TMessage>(TMessage message) where TMessage : IMessage
        {
            try
            {
                var handlers = this.ResolveMessageHandlers<TMessage>();

                if (handlers == null || !handlers.Any())
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);

                return false;
            }
        }

        public bool CanMessageFor<TMessage>(TMessage message, params string[] addresses) where TMessage : IMessage
        {
            try
            {
                var handlers = this.ResolveMessageHandlers<TMessage>();

                var addressableHandlers = handlers.OfType<IAddressable>().Where(x => x.Handles.Intersect(addresses).Any()).Cast<IMessageHandler<TMessage>>();

                if (addressableHandlers == null || !addressableHandlers.Any())
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);

                return false;
            }
        }

        public bool CanQuery<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            try
            {
                var queryHandler = this.Resolve<TQuery, TQueryResult>();

                return queryHandler != null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);

                return false;
            }
        }

        public bool CanQueryFor<TQuery, TQueryResult>(TQuery query, params string[] addresses) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            try
            {
                var queryHandlers = this.ResolveAll<TQuery, TQueryResult>();

                var addressableHandlers = queryHandlers.OfType<IAddressable>().Where(x => x.Handles.Intersect(addresses).Any()).Cast<IQueryHandler<TQuery, TQueryResult>>();

                if (addressableHandlers == null || !addressableHandlers.Any())
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);

                return false;
            }
        }

        public async Task<ICommandResult> Command<TCommand>(TCommand command, bool suppressExceptions = true) where TCommand : ICommand
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                if (command == null)
                {
                    throw new ArgumentNullException(paramName: nameof(command));
                }

                if (command is IContextAware contextAwareCommand)
                {
                    if (this.Context == null)
                    {
                        this.logger.LogWarning(1, "Command {CommandName} Is Context Aware, Context Was Not Set.{correlationId} - {CommandType}", command.EventName, this.CorrelationId, command.GetType());
                    }
                    else
                    {
                        contextAwareCommand.SetContext(dispatcherContext: this.Context);
                    }
                }

                var auditStore = this.serviceProvider.GetService<IAuditStore>();

                if (auditStore != null)
                {
                    this.logger.LogDebug("Audit Store Found - {AuditStoreType}", auditStore.GetType());
                }

                this.logger.LogDebug(1, "Entered Command Dispatcher. {correlationId} - {CommandName} - {CommandType}", this.CorrelationId, command.EventName, command.GetType());
                this.logger.LogTrace("Command Details {@Command}", command);

                ICommandResult result = null;

                var commandHandler = this.Resolve<TCommand>();
                var commandReactors = this.ResolveCommandReactors<TCommand>();
                var commandInterceptors = this.ResolveCommandInterceptors<TCommand>();
                var commandResultReactors = this.ResolveCommandResultReactors<TCommand>();
                var commandValidators = this.ResolveCommandValidators<TCommand>();

                if (commandHandler == null)
                {
                    this.logger.LogError("Command Handler Not Found - {CommandName} - {CommandType}", command.EventName, command.GetType());

                    throw new CommandHandlerNotFoundException(command);
                }

                this.logger.LogDebug(1, "Found Handler - {HandlerType}", commandHandler.GetType());

                if (commandValidators != null)
                {
                    this.logger.LogDebug(1, "Found {CommandResultValidatorCount} Command Validators", commandValidators.Length);
                }

                if (commandResultReactors != null)
                {
                    this.logger.LogDebug(1, "Found {CommandResultReactorCount} Command Result Reactors", commandResultReactors.Length);
                }

                if (commandReactors != null)
                {
                    this.logger.LogDebug(1, "Found {CommandReactorCount} Command Reactors", commandReactors.Length);
                }

                _ = Task.Run(() =>
                {
                    this.logger.LogDebug(1, "Notifying Command Reactors");

                    this.NotifyCommandReactors(commandReactors, command);
                });

                if (commandInterceptors != null)
                {
                    this.logger.LogDebug(1, "Found {CommandInterceptorCount} Command Interceptors", commandInterceptors.Length);

                    foreach (var commandInterceptor in commandInterceptors)
                    {
                        // Using this nested try / catch block to avoid interceptor exceptions breaking the pipeline
                        try
                        {
                            this.logger.LogDebug($"The Command Interceptor: {commandInterceptor.GetType()} Is About To Get Run");

                            await commandInterceptor.Intercept(command);
                        }
                        catch (Exception ex)
                        {
                            this.logger.LogDebug($"The Command Interceptor: {commandInterceptor.GetType()} Raised An Exception");
                            this.logger.LogError(ex.Message, ex);
                        }
                    }
                }

                var validationResults = new List<ValidationResult>();
                var dataAnnotationsValidator = new DataAnnotationsValidator(serviceProvider: this.serviceProvider);

                var validationPassed = dataAnnotationsValidator.TryValidateObjectRecursive(obj: command, validationResults);

                this.logger.LogDebug(1, "Command {CommandName} Initial Validation Result: {Status}", command.EventName, validationPassed);

                if (!validationPassed && validationResults.Count > 0)
                {
                    result = new CommandResult();
                    result.As<CommandResult>().Fail(validationResults);
                }

                IValidationResult validationResult = null;

                if (validationPassed)
                {
                    if (commandValidators != null && commandValidators?.Length > 0)
                    {
                        foreach (var commandValidator in commandValidators)
                        {
                            var intermediateValidationResult = await commandValidator.Validate(command);

                            if (!intermediateValidationResult.Success)
                            {
                                validationResult = intermediateValidationResult;

                                break;
                            }
                        }
                    }
                }

                var finalResult = validationResult ?? (result == null ? await commandHandler.Handle(command) : result.Success ? await commandHandler.Handle(command) : result);

                this.logger.LogDebug(1, "Notifying Command Result Reactors");

                _ = Task.Run(() =>
                {
                    this.logger.LogDebug(1, "Notifying Command Reactors");

                    this.NotifyCommandResultReactors(commandResultReactors, command, finalResult);
                });

                this.logger.LogDebug(1, "Returning From Dispatcher {FinalResult}", finalResult);

                stopWatch.Stop();

                if (auditStore != null)
                {
                    await auditStore.Log(auditContext: new AuditContext(correlationId: this.CorrelationId, runtimeMilliSeconds: stopWatch.ElapsedMilliseconds), command: command, commandResult: finalResult);
                }

                return finalResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);

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

        public async Task Message<TMessage>(TMessage message) where TMessage : IMessage
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            this.logger.LogDebug(2, "Entered Message Dispatcher. {correlationId}", this.CorrelationId);

            var auditStore = this.serviceProvider.GetService<IAuditStore>();

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

        public async Task MessageFor<TMessage>(TMessage message, params string[] addresses) where TMessage : IMessage
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            this.logger.LogDebug(2, "Entered MessageFor Dispatcher. {correlationId}", this.CorrelationId);

            var auditStore = this.serviceProvider.GetService<IAuditStore>();

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

        public async Task<TQueryResult> Query<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                var auditStore = this.serviceProvider.GetService<IAuditStore>();

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

        public async Task<TQueryResult> QueryFor<TQuery, TQueryResult>(TQuery query, params string[] addresses) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                this.logger.LogDebug(3, "Entered Query Dispatcher. {correlationId}", this.CorrelationId);

                var auditStore = this.serviceProvider.GetService<IAuditStore>();

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

        private void NotifyCommandReactors<TCommand>(ICommandReactor<TCommand>[] commandReactors, TCommand command) where TCommand : ICommand
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

        private void NotifyCommandResultReactors<TCommand, TCommandResult>(ICommandResultReactor<TCommand>[] commandResultReactors, TCommand command, TCommandResult commandResult)
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
                        this.logger.LogError(5, "Error Occurred NotifyCommandResultReactors. {correlationId}", this.CorrelationId, ex);
                    }
                });
            }
        }

        private ICommandResult CreateFromException(Exception ex)
        {
            var exceptionResult = new CommandExceptionResult(ex.Message, ex);

            return exceptionResult;
        }

        private ICommandHandler<TCommand> Resolve<TCommand>() where TCommand : ICommand
        {
            var commandHandler = this.serviceProvider.GetService<ICommandHandler<TCommand>>();

            return commandHandler;
        }

        private ICommandReactor<TCommand>[] ResolveCommandReactors<TCommand>() where TCommand : ICommand
        {
            var scope = this.serviceScopeFactory.CreateScope();
            var commandReactors = scope.ServiceProvider.GetServices<ICommandReactor<TCommand>>();

            return commandReactors.ToArray();
        }

        private ICommandInterceptor<TCommand>[] ResolveCommandInterceptors<TCommand>() where TCommand : ICommand
        {
            var commandInterceptors = this.serviceProvider.GetServices<ICommandInterceptor<TCommand>>();

            return commandInterceptors.ToArray();
        }

        private ICommandResultReactor<TCommand>[] ResolveCommandResultReactors<TCommand>() where TCommand : ICommand
        {
            var scope = this.serviceScopeFactory.CreateScope();
            var commandResultReactors = scope.ServiceProvider.GetServices<ICommandResultReactor<TCommand>>();

            return commandResultReactors.ToArray();
        }

        private IMessageHandler<TMessage>[] ResolveMessageHandlers<TMessage>() where TMessage : IMessage
        {
            var commandHandlers = this.serviceProvider.GetServices<IMessageHandler<TMessage>>();

            return commandHandlers.ToArray();
        }

        private IValidator<TCommand>[] ResolveCommandValidators<TCommand>() where TCommand : ICommand
        {
            var commandValidators = this.serviceProvider.GetServices<IValidator<TCommand>>();

            return commandValidators.ToArray();
        }

        private IQueryHandler<TQuery, TQueryResult> Resolve<TQuery, TQueryResult>()
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            var queryHandler = this.serviceProvider.GetService<IQueryHandler<TQuery, TQueryResult>>();

            return queryHandler;
        }

        private IQueryHandler<TQuery, TQueryResult>[] ResolveAll<TQuery, TQueryResult>()
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            var queryHandler = this.serviceProvider.GetServices<IQueryHandler<TQuery, TQueryResult>>();

            return queryHandler.ToArray();
        }

        #endregion Private Methods
    }
}