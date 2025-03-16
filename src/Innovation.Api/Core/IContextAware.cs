namespace Innovation.Api.Core
{
    using System.Diagnostics.CodeAnalysis;

    using Innovation.Api.Dispatching;

    public interface IContextAware
    {
        void SetContext([DisallowNull] IDispatcherContext dispatcherContext);
    }
}
