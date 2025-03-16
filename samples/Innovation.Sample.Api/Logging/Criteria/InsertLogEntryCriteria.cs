namespace Innovation.Sample.Api.Logging.Criteria
{
    using System.ComponentModel.DataAnnotations;

    using Innovation.Sample.Api.Logging.Statics;

    public class InsertLogEntryCriteria
    {
        #region Constructor

        public InsertLogEntryCriteria(string message)
        {
            this.Message = message;
        }

        #endregion Constructor

        #region Properties

        [Required(ErrorMessage = InsertLogEntryStatics.MessageRequiredErrorMessage)]
        [MinLength(length: InsertLogEntryStatics.MessageMinLength, ErrorMessage = InsertLogEntryStatics.MessageMinErrorMessage)]
        [MaxLength(length: InsertLogEntryStatics.MessageMaxLength, ErrorMessage = InsertLogEntryStatics.MessageMaxErrorMessage)]
        public string Message { get; }

        #endregion Properties
    }
}
