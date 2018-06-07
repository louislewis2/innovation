namespace Innovation.Sample.Api.Logging.Commands
{
    using Innovation.Api.Commanding;

    public class InsertLogEntry : ICommand
    {
        #region Constructor

        public InsertLogEntry(string message)
        {
            this.Message = message;
        }

        #endregion Constructor

        #region Properties

        public string Message { get; }

        #endregion Properties

        #region ICommand

        public string EventName => "Create Log Entry";

        #endregion ICommand
    }
}
