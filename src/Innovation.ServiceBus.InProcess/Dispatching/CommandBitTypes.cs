namespace Innovation.ServiceBus.InProcess.Dispatching
{
    public enum CommandBitTypes
    {
        Unknown = 0,
        CommandHandlerRegistered = 1,
        CommandValidator = 2,
        CommandReactor = 3,
        CommandResultReactor = 4,
        CommandInterceptor = 5,
        ContextAware = 6,
        CorrelationIdAware = 7,
        IsValidationEnabled = 8
    }
}
