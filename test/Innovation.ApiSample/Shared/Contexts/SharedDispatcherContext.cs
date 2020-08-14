namespace Innovation.ApiSample.Shared.Contexts
{
    using Innovation.Api.Dispatching;

    public class SharedDispatcherContext : IDispatcherContext
    {
        #region Properties

        public string CorrelationId { get; private set; }

        #endregion Properties

        #region Methods

        public void SetCorrelationId(string correlationId)
        {
            this.CorrelationId = correlationId;
        }

        #endregion Methods
    }
}
