namespace Innovation.Sample.Api.Logging.Statics
{
    public class InsertLogEntryStatics
    {
        // Message
        public const string MessageRequiredErrorMessage = "Is mandatory";

        public const int MessageMinLength = 10;
        public const string MessageMinErrorMessage = "Must be at least 10 characters";

        public const int MessageMaxLength = 100;
        public const string MessageMaxErrorMessage = "Cannot exceed 100 characters";
    }
}
