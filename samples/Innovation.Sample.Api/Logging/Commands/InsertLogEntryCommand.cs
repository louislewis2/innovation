namespace Innovation.Sample.Api.Logging.Commands
{
    using Innovation.Api.Commanding;

    using Innovation.Sample.Api.Logging.Criteria;

    public class InsertLogEntryCommand : ICommand
    {
        #region Constructor

        public InsertLogEntryCommand(InsertLogEntryCriteria insertLogEntryCriteria)
        {
            this.Criteria = insertLogEntryCriteria;
        }

        #endregion Constructor

        #region Properties

        public InsertLogEntryCriteria Criteria { get; }

        #endregion Properties

        #region ICommand

        public string EventName => nameof(InsertLogEntryCommand);

        #endregion ICommand
    }
}
