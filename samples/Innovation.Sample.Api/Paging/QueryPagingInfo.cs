namespace Innovation.Sample.Api.Paging
{
    using System;

    public class QueryPagingInfo
    {
        #region Constructor

        public QueryPagingInfo(int currentPage, int pageSize, int totalItemCount)
        {
            CurrentPage = currentPage;
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
        }

        #endregion Constructor

        #region Properties

        public int? TotalItemCount { get; }
        public int PageSize { get; }
        public int CurrentPage { get; }

        public int? TotalPages => this.TotalItemCount.HasValue ? (int)Math.Ceiling((double)TotalItemCount / PageSize) : (int?)null;
        public bool? HasNext => this.TotalItemCount.HasValue ? (this.CurrentPage < this.TotalPages) : (bool?)null;
        public bool? HasPrevious => this.TotalItemCount.HasValue ? (this.CurrentPage > 1) : (bool?)null;

        #endregion Properties
    }
}
