namespace Innovation.ServiceBus.InProcess.Exceptions
{
    using System;

    using Api.Querying;

    public class QueryHandlerNotFoundException : Exception
    {
        #region Fields

        private readonly IQuery query;

        #endregion Fields

        #region Constructor

        public QueryHandlerNotFoundException(IQuery query) : base($"Query.Name: {query.EventName} - Query.Type: {query.GetType()}")
        {
            this.query = query;
        }
        #endregion Constructor

        #region Properties

        public IQuery Query => this.query;
        public string QueryName => this.query.EventName;
        public Type QueryType => this.query.GetType();

        #endregion Properties
    }
}