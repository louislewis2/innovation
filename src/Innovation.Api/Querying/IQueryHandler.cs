namespace Innovation.Api.Querying
{
    using System.Threading.Tasks;

    /// <summary>
    /// This interface is for query handlers.
    /// All query handlers need to implement this interface.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public interface IQueryHandler<in TQuery, TQueryResult>
        where TQuery : IQuery
        where TQueryResult : IQueryResult
    {
        Task<TQueryResult> Handle(TQuery query);
    }
}
