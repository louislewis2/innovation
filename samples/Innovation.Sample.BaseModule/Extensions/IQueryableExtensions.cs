namespace System.Linq
{
    using Innovation.Sample.Api.Paging;

    public static class IQueryableExtensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, QueryPage queryPage)
        {
            return queryable.Skip((queryPage.Page - 1) * queryPage.PageSize).Take(queryPage.PageSize);
        }
    }
}
