namespace Innovation.Api.Dispatching
{
    public interface IDispatcherContext
    {
        object DispatcherContext { get; set; }
        string CorrelationId { get; set; }
    }
}
