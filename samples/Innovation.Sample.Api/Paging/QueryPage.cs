namespace Innovation.Sample.Api.Paging
{
    using Innovation.Api.Querying;

    public class QueryPage : IQuery
    {
        #region Constructor

        public QueryPage()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        #endregion Constructor

        #region Properties

        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool IncludeServerCount { get; set; }

        #endregion Properties

        #region IQuery

        public string EventName => "Get paged results";

        #endregion IQuery
    }
}
