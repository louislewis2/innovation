namespace Innovation.ServiceBus.InProcess.Dispatching
{
    using System;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;

    internal static class DispatcherLogging
    {
        #region Fields

        public static readonly EventId commandEventId = new EventId(1, "Command");

        private static readonly Action<ILogger, Exception> commandParameterNull = LoggerMessage.Define(
            LogLevel.Error,
            commandEventId,
            "Command Cannot Be Null");

        private static readonly Action<ILogger, string, string, Type, Exception> contextNotSet = LoggerMessage.Define<string, string, Type>(
            LogLevel.Debug,
            commandEventId,
            "Command {CommandName} Is Context Aware, Context Was Not Set.{correlationId} - {CommandType}");

        private static readonly Action<ILogger, Type, Exception> auditStoreFound = LoggerMessage.Define<Type>(
            LogLevel.Debug,
            commandEventId,
            "Audit Store Found - {AuditStoreType}");

        private static readonly Action<ILogger, string, string, Type, Exception> enteredCommandDispatcher = LoggerMessage.Define<string, string, Type>(
            LogLevel.Debug,
            commandEventId,
            "Entered Command Dispatcher. {correlationId} - {CommandName} - {CommandType}");

        private static readonly Action<ILogger, ICommand, Exception> commandDetails = LoggerMessage.Define<ICommand>(
            LogLevel.Trace,
            commandEventId,
            "Command Details {@Command}");

        private static readonly Action<ILogger, string, Type, Exception> commandHandlerNotFound = LoggerMessage.Define<string, Type>(
            LogLevel.Error,
            commandEventId,
            "Command Handler Not Found - {CommandName} - {CommandType}");

        private static readonly Action<ILogger, Type, Exception> commandHandlerFound = LoggerMessage.Define<Type>(
            LogLevel.Debug,
            commandEventId,
            "Found Handler - {HandlerType}");

        private static readonly Action<ILogger, int, Exception> commandValidatorsFound = LoggerMessage.Define<int>(
            LogLevel.Debug,
            commandEventId,
            "Found {CommandValidatorCount} Command Validators");

        private static readonly Action<ILogger, int, Exception> commandResultReactorsFound = LoggerMessage.Define<int>(
            LogLevel.Debug,
            commandEventId,
            "Found {CommandResultReactorCount} Command Result Reactors");

        private static readonly Action<ILogger, int, Exception> commandReactorsFound = LoggerMessage.Define<int>(
            LogLevel.Debug,
            commandEventId,
            "Found {CommandReactorCount} Command Reactors");

        private static readonly Action<ILogger, Exception> notifyingCommandReactors = LoggerMessage.Define(
            LogLevel.Debug,
            commandEventId,
            "Notifying Command Reactors");

        private static readonly Action<ILogger, int, Exception> commandInterceptorsFound = LoggerMessage.Define<int>(
            LogLevel.Debug,
            commandEventId,
            "Found {CommandInterceptorCount} Command Interceptors");

        private static readonly Action<ILogger, Type, Exception> commandInterceptorGoingToRun = LoggerMessage.Define<Type>(
            LogLevel.Debug,
            commandEventId,
            "The Command Interceptor: {CommandInterceptorType} Is About To Get Run");

        private static readonly Action<ILogger, Type, Exception> commandInterceptorRaisedException = LoggerMessage.Define<Type>(
            LogLevel.Debug,
            commandEventId,
            "The Command Interceptor: {CommandInterceptorType} Raised An Exception");

        private static readonly Action<ILogger, string, bool, Exception> commandInitialValidationResult = LoggerMessage.Define<string, bool>(
            LogLevel.Debug,
            commandEventId,
            "Command {CommandName} Initial Validation Result: {Status}");

        private static readonly Action<ILogger, Exception> notifyingCommandResultReactors = LoggerMessage.Define(
            LogLevel.Debug,
            commandEventId,
            "Notifying Command Result Reactors");

        private static readonly Action<ILogger, bool,Exception> returningFromDispatcher = LoggerMessage.Define<bool>(
            LogLevel.Debug,
            commandEventId,
            "Returning From Dispatcher {FinalResult}");

        #endregion Fields

        #region Methods

        public static void CommandParameterNull(ILogger logger) => commandParameterNull(logger, null);
        public static void ContextNotSet(ILogger logger, string commandName, string correlationId, Type commandType) => contextNotSet(logger, commandName, correlationId, commandType, null);
        public static void AuditStoreFound(ILogger logger, Type auditStoreType) => auditStoreFound(logger, auditStoreType, null);
        public static void EnteredCommandDispatcher(ILogger logger, string correlationId, string commandName, Type commandType) => enteredCommandDispatcher(logger, correlationId, commandName, commandType, null);
        public static void CommandDetail(ILogger logger, ICommand command) => commandDetails(logger, command, null);
        public static void CommandHandlerNotFound(ILogger logger, string eventName, Type commandType) => commandHandlerNotFound(logger, eventName, commandType, null);
        public static void CommandHandlerFound(ILogger logger, Type commandHandlerType) => commandHandlerFound(logger, commandHandlerType, null);
        public static void CommandValidatorsFound(ILogger logger, int commandValidatorCount) => commandValidatorsFound(logger, commandValidatorCount, null);
        public static void CommandResultReactorsFound(ILogger logger, int commandResultReactorCount) => commandResultReactorsFound(logger, commandResultReactorCount, null);
        public static void CommandReactorsFound(ILogger logger, int commandReactorCount) => commandReactorsFound(logger, commandReactorCount, null);
        public static void NotifyingCommandReactors(ILogger logger) => notifyingCommandReactors(logger, null);
        public static void CommandInterceptorsFound(ILogger logger, int commandInterceptorCount) => commandInterceptorsFound(logger, commandInterceptorCount, null);
        public static void CommandInterceptorGoingToRun(ILogger logger, Type commandInterceptorType) => commandInterceptorGoingToRun(logger, commandInterceptorType, null);
        public static void CommandInterceptorRaisedException(ILogger logger, Type commandInterceptorType) => commandInterceptorRaisedException(logger, commandInterceptorType, null);
        public static void CommandInitialValidationResult(ILogger logger, string eventName, bool isValid) => commandInitialValidationResult(logger, eventName, isValid, null);
        public static void NotifyingCommandResultReactors(ILogger logger) => notifyingCommandResultReactors(logger, null);
        public static void ReturningFromDispatcher(ILogger logger, bool status) => returningFromDispatcher(logger, status, null);

        #endregion Methods
    }
}
