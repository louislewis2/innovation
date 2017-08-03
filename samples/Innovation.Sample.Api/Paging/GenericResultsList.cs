namespace Innovation.Sample.Api.Paging
{
    using Innovation.Api.Querying;

    public class GenericResultsList<T> : IQueryResult
    {
        #region Constructor

        public GenericResultsList(T[] items, QueryPagingInfo paging)
        {
            this.Items = items;
            this.Paging = paging;
        }

        #endregion Constructor

        #region Properties

        public T[] Items { get; }
        public QueryPagingInfo Paging { get; }

        #endregion Properties
    }
}
