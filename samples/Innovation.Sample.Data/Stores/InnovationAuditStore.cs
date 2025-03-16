namespace Innovation.Sample.Data.Stores
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Innovation.Api.Querying;
    using Innovation.Api.Messaging;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;

    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Data.Anemics.Auditing;
    using Innovation.Sample.Infrastructure.Settings;

    public class InnovationAuditStore<TAuditDbContext> : IAuditStore where TAuditDbContext : AuditDbContextBase<TAuditDbContext>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly AuditDbContextBase<TAuditDbContext> auditDbContext;
        private readonly InnovationAuditSettings innovationAuditSettings;

        #endregion Fields

        #region Constructor

        public InnovationAuditStore(
            ILogger<InnovationAuditStore<TAuditDbContext>> logger,
            AuditDbContextBase<TAuditDbContext> auditDbContext,
            IOptions<InnovationAuditSettings> innovationAuditSettingsOptions)
        {
            this.logger = logger;
            this.auditDbContext = auditDbContext;
            this.innovationAuditSettings = innovationAuditSettingsOptions.Value;
        }

        #endregion Constructor

        #region Methods

        public async Task Log(AuditContext auditContext, ICommand command, ICommandResult commandResult)
        {
            if (!this.innovationAuditSettings.EnableCommandAudits)
            {
                return;
            }

            await this.InsertCommandAudit(
                auditContext: auditContext,
                command: command,
                commandResult: commandResult);
        }

        public async Task Log(AuditContext auditContext, IQuery query)
        {
            if (!this.innovationAuditSettings.EnableQueryAudits)
            {
                return;
            }

            await this.InsertQueryAudit(
                auditContext: auditContext,
                query: query);
        }

        public Task Log(AuditContext auditContext, IMessage message)
        {
            return Task.CompletedTask;
        }

        #endregion Methods

        #region Private Methods

        private async Task InsertCommandAudit(AuditContext auditContext, ICommand command, ICommandResult commandResult)
        {
            try
            {
                var commandContext = JsonSerializer.Serialize(value: command);
                var commandResultContext = JsonSerializer.Serialize(value: commandResult);

                var commandAuditEntryAnemic = CommandAuditEntryAnemic.New(
                    id: Guid.NewGuid(),
                    name: command.EventName,
                    commandContext: commandContext,
                    commandResultContext: commandResultContext,
                    correlationId: auditContext.CorrelationId,
                    runtimeMilliSeconds: auditContext.RuntimeMilliSeconds);

                this.auditDbContext.Add(entity: commandAuditEntryAnemic);
                await this.auditDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(exception: ex, message: ex.GetInnerMostMessage());
            }
        }

        private async Task InsertQueryAudit(AuditContext auditContext, IQuery query)
        {
            try
            {
                var queryContext = JsonSerializer.Serialize(value: query);

                var queryAuditEntryAnemic = QueryAuditEntryAnemic.New(
                    id: Guid.NewGuid(),
                    name: query.EventName,
                    queryContext: queryContext,
                    correlationId: auditContext.CorrelationId,
                    runtimeMilliSeconds: auditContext.RuntimeMilliSeconds);

                this.auditDbContext.Add(entity: queryAuditEntryAnemic);
                await this.auditDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(exception: ex, message: ex.GetInnerMostMessage());
            }
        }

        #endregion Private Methods
    }
}
