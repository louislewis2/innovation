namespace Innovation.Api.Dispatching
{
    using System.Threading.Tasks;

    using Innovation.Api.Querying;
    using Innovation.Api.Messaging;
    using Innovation.Api.Commanding;

    public interface IAuditStore
    {
        Task Log(AuditContext auditContext, ICommand command, ICommandResult commandResult);
        Task Log(AuditContext auditContext, IQuery query);
        Task Log(AuditContext auditContext, IMessage message);
    }
}
