namespace Innovation.Api.Dispatching
{
    using System;
    using System.Threading.Tasks;

    using Innovation.Api.Querying;
    using Innovation.Api.Messaging;
    using Innovation.Api.Commanding;

    public interface IAuditStore
    {
        Task Log(Guid correlationId, ICommand command, ICommandResult commandResult);
        Task Log(Guid correlationId, IQuery query);
        Task Log(Guid correlationId, IMessage message);
    }
}
