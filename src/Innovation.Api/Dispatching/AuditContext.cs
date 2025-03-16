namespace Innovation.Api.Dispatching
{
    using System.Diagnostics.CodeAnalysis;

    public class AuditContext
    {
        #region Constructor

        public AuditContext([DisallowNull] string correlationId, long runtimeMilliSeconds)
        {
            this.CorrelationId = correlationId;
            this.RuntimeMilliSeconds = runtimeMilliSeconds;
        }

        #endregion Constructor

        #region Properties

        public string CorrelationId { get; }
        public long RuntimeMilliSeconds { get; }

        #endregion Properties
    }
}
