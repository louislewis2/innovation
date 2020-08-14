namespace Innovation.Api.Commanding
{
    using Innovation.Api.Dispatching;

    public interface IContextAware
    {
        void SetContext(IDispatcherContext dispatcherContext);
    }
}
