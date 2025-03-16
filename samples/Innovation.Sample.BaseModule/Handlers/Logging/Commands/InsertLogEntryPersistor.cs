namespace Innovation.Sample.BaseModule.Handlers.Logging.Commands
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Logging.Commands;
    using Innovation.Sample.Data.Anemics.Logging;

    public class InsertLogEntryPersistor : ICommandHandler<InsertLogEntryCommand>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly PrimaryContext primaryContext;

        #endregion Fields

        #region Constructor

        public InsertLogEntryPersistor(ILogger<InsertLogEntryPersistor> logger, PrimaryContext primaryContext)
        {
            this.logger = logger;
            this.primaryContext = primaryContext;
        }

        #endregion Constructor

        #region Methods

        public async ValueTask<ICommandResult> Handle(InsertLogEntryCommand command)
        {
            return await Persist(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(InsertLogEntryCommand command)
        {
            var result = new CommandResult();

            try
            {
                var logEntry = LogEntryAnemic.New(
                    id: Guid.NewGuid(),
                    message: command.Criteria.Message);

                primaryContext.LogEntries.Add(entity: logEntry);
                await primaryContext.SaveChangesAsync();

                result.SetRecord(recordId: logEntry.Id);

                return result;
            }
            catch (Exception ex)
            {
                result.Fail(ex: ex);
            }

            return result;
        }

        #endregion Private Methods
    }
}
