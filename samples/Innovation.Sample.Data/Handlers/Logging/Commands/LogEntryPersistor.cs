namespace Innovation.Sample.Data.Handlers.Logging.Commands
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using Innovation.Sample.Data.Anemics;
    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Logging.Commands;

    public class LogEntryPersistor : ICommandHandler<InsertLogEntry>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly ExampleContext context;

        #endregion Fields

        #region Constructor

        public LogEntryPersistor(ILogger<LogEntryPersistor> logger, ExampleContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        #endregion Constructor

        #region Methods

        public async Task<ICommandResult> Handle(InsertLogEntry command)
        {
            return await this.Persist(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task<ICommandResult> Persist(InsertLogEntry command)
        {
            var result = new CommandResult();

            try
            {
                var logEntry = new AnemicLogEntry()
                {
                    Id = Guid.NewGuid(),
                    Message = command.Message
                };

                context.LogEntries.Add(logEntry);

                await context.SaveChangesAsync();

                result.SetRecord(logEntry.Id);

                return result;
            }
            catch (Exception ex)
            {
                result.Fail(ex);
            }

            return result;
        }

        #endregion Private Methods
    }
}
