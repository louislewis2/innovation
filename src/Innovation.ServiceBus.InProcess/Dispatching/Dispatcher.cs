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
    using Api.Querying;
    using Api.Reactions;
    using Api.Messaging;
    using Api.Commanding;
    using Api.Dispatching;
    using Api.Interceptors;
    using Api.CommandHelpers;

    /// <summary>
    /// This is the implementation of the dispatcher for the in process service bus.
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        #region Fields

        private readonly ILogger logger;
        private readonly Guid instanceIdentifier = Guid.NewGuid();
        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructor

        public Dispatcher(
            ILogger<Dispatcher> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructor

        #region Methods

        public bool CanCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            try
            {
                var commandHandler = this.Resolve<TCommand>();

                return commandHandler == null ? false : true;
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

                return queryHandler == null ? false : true;
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
            try
            {
                this.logger.LogDebug(1, "Entered Command Dispatcher. {instanceIdentifier} - {CommandName} - {CommandType}", this.instanceIdentifier, command.EventName, command.GetType());
                this.logger.LogTrace("Command Details {@Command}", command);

                ICommandResult result = null;

                var commandHandler = this.Resolve<TCommand>();
                var commandReactors = this.ResolveCommandReactors<TCommand>();
                var commandInterceptors = this.ResolveCommandInterceptors<TCommand>();
                var commandResultReactors = this.ResolveCommandResultReactors<TCommand>();

                if (commandHandler == null)
                {
                    this.logger.LogError("Command Handler Not Found - {CommandName} - {CommandType}", command.EventName, command.GetType());
                    throw new CommandHandlerNotFoundException(command);
                }

                this.logger.LogDebug(1, "Found Handler - {HandlerType}", commandHandler.GetType());

                if (commandResultReactors != null)
                {
                    this.logger.LogDebug(1, "Found {CommandResultReactorCount} Command Result Reactors", commandResultReactors.Length);
                }

                if (commandReactors != null)
                {
                    this.logger.LogDebug(1, "Found {CommandReactorCount} Command Reactors", commandReactors.Length);
                }

                await Task.Run(async () =>
                {
                    this.logger.LogDebug(1, "Notifying Command Reactors");
                    await this.NotifyCommandReactors(commandReactors, command);
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
                var validationContext = new ValidationContext(command, this.serviceProvider, null);
                var validationPassed = Validator.TryValidateObject(command, validationContext, validationResults, true);

                this.logger.LogDebug(1, "Command {CommandName} Initial Validation Result: {Status}", command.EventName, validationPassed);

                if (!validationPassed && validationResults.Count > 0)
                {
                    result = new CommandResult();
                    result.As<CommandResult>().Fail(validationResults);
                }

                if (command is IValidatableObject)
                {
                    this.logger.LogDebug(1, "Command {CommandName} Requires Self Validation", command.EventName);

                    var selfValidationResults = ((IValidatableObject)command).Validate(validationContext);

                    if (selfValidationResults != null && selfValidationResults.Count() > 0)
                    {
                        if (result == null)
                        {
                            result = new CommandResult();
                        }

                        result.As<CommandResult>().Fail(selfValidationResults);
                    }
                }

                var finalResult = result == null ? await commandHandler.Handle(command) : result.Success ? await commandHandler.Handle(command) : result;

                await Task.Run(async () =>
                {
                    this.logger.LogDebug(1, "Notifying Command Result Reactors");
                    await this.NotifyCommandResultReactors(commandResultReactors, command, finalResult);
                });

                this.logger.LogDebug(1, "Returning From Dispatcher {FinalResult}", finalResult);

                return finalResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                if (suppressExceptions)
                {
                    return this.CreateFromException(ex, command);
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Message<TMessage>(TMessage message) where TMessage : IMessage
        {
            this.logger.LogDebug(2, "Entered Message Dispatcher. {instanceIdentifier}", this.instanceIdentifier);

            var handlers = this.ResolveMessageHandlers<TMessage>();

            foreach (var handler in handlers)
            {
                await Task.Run(() => handler.Handle(message));
            }
        }

        public async Task MessageFor<TMessage>(TMessage message, params string[] addresses) where TMessage : IMessage
        {
            this.logger.LogDebug(2, "Entered MessageFor Dispatcher. {instanceIdentifier}", this.instanceIdentifier);

            var handlers = this.ResolveMessageHandlers<TMessage>();

            var addressableHandlers = handlers.OfType<IAddressable>().Where(x => x.Handles.Intersect(addresses).Any()).Cast<IMessageHandler<TMessage>>();

            if (addressableHandlers == null || !addressableHandlers.Any())
            {
                this.logger.LogError("Addressable Message Handlers Not Found - {MessageName} - {MessageType} - {Addresses}", message.EventName, message.GetType(), addresses);
                return;
            }

            foreach (var handler in addressableHandlers)
            {
                await Task.Run(() => handler.Handle(message));
            }
        }

        public async Task<TQueryResult> Query<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            try
            {
                this.logger.LogDebug(3, "Entered Query Dispatcher. {instanceIdentifier}", this.instanceIdentifier);

                var queryHandler = this.Resolve<TQuery, TQueryResult>();

                if (queryHandler == null)
                {
                    this.logger.LogError("Query Handler Not Found - {QueryName} - {QueryType} - {ResultType}", query.EventName, query.GetType(), typeof(TQueryResult));
                    throw new QueryHandlerNotFoundException(query);
                }

                this.logger.LogDebug(3, "Found Handler - {HandlerType} - {ResultType}", query.GetType(), typeof(TQueryResult));

                this.logger.LogDebug(3, "Calling QueryHandler");

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
            try
            {
                this.logger.LogDebug(3, "Entered Query Dispatcher. {instanceIdentifier}", this.instanceIdentifier);

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

        private async Task NotifyCommandReactors<TCommand>(ICommandReactor<TCommand>[] commandReactors, TCommand command) where TCommand : ICommand
        {
            this.logger.LogDebug(4, "Entered NotifyCommandReactors. {instanceIdentifier}", this.instanceIdentifier);

            foreach (var reactor in commandReactors)
            {
                await Task.Run(() =>
                {
                    reactor.React(command);
                });
            }
        }

        private async Task NotifyCommandResultReactors<TCommand, TCommandResult>(ICommandResultReactor<TCommand>[] commandResultReactors, TCommand command, TCommandResult commandResult)
            where TCommand : ICommand
            where TCommandResult : ICommandResult
        {
            this.logger.LogDebug(5, "Entered NotifyCommandResultReactors. {instanceIdentifier}", this.instanceIdentifier);

            foreach (var reactor in commandResultReactors)
            {
                await Task.Run(() =>
                {
                    reactor.React(commandResult, command);
                });
            }
        }

        private ICommandResult CreateFromException(Exception ex, ICommand command)
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
            var commandReactors = this.serviceProvider.GetServices<ICommandReactor<TCommand>>();

            return commandReactors.ToArray();
        }

        private ICommandInterceptor<TCommand>[] ResolveCommandInterceptors<TCommand>() where TCommand : ICommand
        {
            var commandInterceptors = this.serviceProvider.GetServices<ICommandInterceptor<TCommand>>();

            return commandInterceptors.ToArray();
        }

        private ICommandResultReactor<TCommand>[] ResolveCommandResultReactors<TCommand>() where TCommand : ICommand
        {
            var commandResultReactors = this.serviceProvider.GetServices<ICommandResultReactor<TCommand>>();

            return commandResultReactors.ToArray();
        }

        private IMessageHandler<TMessage>[] ResolveMessageHandlers<TMessage>() where TMessage : IMessage
        {
            var commandHandlers = this.serviceProvider.GetServices<IMessageHandler<TMessage>>();

            return commandHandlers.ToArray();
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