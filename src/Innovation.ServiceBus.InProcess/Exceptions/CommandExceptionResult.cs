namespace Innovation.ServiceBus.InProcess.Exceptions
{
    using System;

    using Innovation.Api.Commanding;

    public class CommandExceptionResult : ICommandResult
    {
        #region Constructor

        public CommandExceptionResult(string message, Exception exception = null)
        {
            this.Message = message;
            this.Exception = exception;
        }

        public CommandExceptionResult(Exception exception)
        {
            this.Message = exception.Message;
            this.Exception = exception;
        }

        #endregion Constructor

        #region Properties

        public bool Success => false;

        public string Message { get; }

        public Exception Exception { get; }

        #endregion Properties

        #region Overrides

        public override string ToString()
        {
            return this.Message;
        }

        #endregion Overrides
    }
}
