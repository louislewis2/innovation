namespace Innovation.Integration.AspNetCore
{
    public class CorrelationIdOptions
    {
        #region Fields

        private const string DefaultHeader = "X-Correlation-ID";

        #endregion Fields

        #region Properties

        /// <summary>
        /// The header field name where the correlation ID will be retrieved from
        /// </summary>
        public string Header { get; set; } = DefaultHeader;

        #endregion Properties
    }
}
