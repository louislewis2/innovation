namespace Innovation.Sample.Api.Messages
{
    using System;

    using Innovation.Api.Messaging;

    public class ExceptionMessage : IMessage
    {
        #region Properties

        public Exception Exception { get; set; }

        #endregion Properties

        #region IMessage

        public string EventName => "Exception Occurred Message";

        #endregion IMessage
    }
}
