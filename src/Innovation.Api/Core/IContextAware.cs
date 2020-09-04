namespace Innovation.Api.Core
{
    using Innovation.Api.Dispatching;

    public interface IContextAware
    {
        void SetContext(IDispatcherContext dispatcherContext);
    }
}
