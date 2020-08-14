namespace Innovation.Api.Dispatching
{
    public interface IDispatcherContext
    {
        void SetCorrelationId(string correlationId);
    }
}
