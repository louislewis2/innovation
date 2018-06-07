namespace Innovation.SampleApi.Consumer.Handlers.Vendors.Commands
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;

    using Innovation.ApiSample.Vendors.Commands;

    public class InsertVendorPersistor : PersistorBase<InsertVendor>
    {
        #region Constructor

        public InsertVendorPersistor(ILogger<InsertVendorPersistor> logger) : base(logger: logger)
        {
        }

        #endregion Constructor

        #region Methods

        public override Task<ICommandResult> Persist()
        {
            return Task.FromResult(this.ReturnError(errorMessage: "Test Error Message"));
        }

        #endregion Methods
    }
}
