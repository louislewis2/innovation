namespace Innovation.Sample.Data.Anemics.Logging
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Innovation.Sample.Api.Logging.Statics;

    public class LogEntryAnemic : AnemicBase
    {
        #region Constructor

        public LogEntryAnemic(Guid id, string message) : base(id)
        {
            this.Message = message;
        }

        #endregion Constructor

        #region Properties

        [Required(ErrorMessage = InsertLogEntryStatics.MessageRequiredErrorMessage)]
        [MinLength(length: InsertLogEntryStatics.MessageMinLength, ErrorMessage = InsertLogEntryStatics.MessageMinErrorMessage)]
        [MaxLength(length: InsertLogEntryStatics.MessageMaxLength, ErrorMessage = InsertLogEntryStatics.MessageMaxErrorMessage)]
        public string Message { get; set; }

        #endregion Properties

        #region Methods

        public static LogEntryAnemic New(Guid id, string message)
        {
            return new LogEntryAnemic(
                id: id,
                message:  message);
        }

        #endregion Methods
    }
}
