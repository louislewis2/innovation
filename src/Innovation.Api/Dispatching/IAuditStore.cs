namespace Innovation.Api.Dispatching
{
    using System;
    using System.Threading.Tasks;

    using Innovation.Api.Querying;
    using Innovation.Api.Messaging;
    using Innovation.Api.Commanding;

    public interface IAuditStore
    {
        Task Log(string correlationId, ICommand command, ICommandResult commandResult);
        Task Log(string correlationId, IQuery query);
        Task Log(string correlationId, IMessage message);
    }
}
