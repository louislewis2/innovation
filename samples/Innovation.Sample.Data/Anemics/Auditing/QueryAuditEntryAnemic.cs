namespace Innovation.Sample.Data.Anemics.Auditing
{
    using System;

    using System.ComponentModel.DataAnnotations.Schema;

    [Table("QueryAuditEntries")]
    public class QueryAuditEntryAnemic : AnemicBase
    {
        #region Constructor

        public QueryAuditEntryAnemic(
            Guid id,
            string name,
            string queryContext,
            string correlationId,
            long runtimeMilliSeconds) : base(id)
        {
            this.Name = name;
            this.QueryContext = queryContext;
            this.CorrelationId = correlationId;
            this.RuntimeMilliSeconds = runtimeMilliSeconds;
        }

        #endregion Constructor

        #region Properties

        public string Name { get; set; }
        public string QueryContext { get; set; }
        public string CorrelationId { get; set; }
        public long RuntimeMilliSeconds { get; set; }

        #endregion Properties

        #region Methods

        public static QueryAuditEntryAnemic New(
            Guid id,
            string name,
            string queryContext,
            string correlationId,
            long runtimeMilliSeconds)
        {
            return new QueryAuditEntryAnemic(
                id: id,
                name: name,
                queryContext: queryContext,
                correlationId: correlationId,
                runtimeMilliSeconds: runtimeMilliSeconds);
        }

        #endregion Methods
    }
}
