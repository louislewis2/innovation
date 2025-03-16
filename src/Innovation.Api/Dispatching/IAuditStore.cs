namespace Innovation.Api.Dispatching
{
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;

    using Innovation.Api.Querying;
    using Innovation.Api.Messaging;
    using Innovation.Api.Commanding;

    public interface IAuditStore
    {
        Task Log([DisallowNull] AuditContext auditContext, [DisallowNull] ICommand command, [DisallowNull] ICommandResult commandResult);
        Task Log([DisallowNull] AuditContext auditContext, [DisallowNull] IQuery query);
        Task Log([DisallowNull] AuditContext auditContext, [DisallowNull] IMessage message);
    }
}
