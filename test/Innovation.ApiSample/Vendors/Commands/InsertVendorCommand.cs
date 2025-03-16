namespace Innovation.ApiSample.Vendors.Commands
{
    using Innovation.Api.Commanding;

    using Criteria;

    public class InsertVendorCommand : ICommand
    {
        #region Constructor

        public InsertVendorCommand(VendorCriteria vendorCriteria)
        {
            this.Criteria = vendorCriteria;
        }

        #endregion Constructor

        #region Properties

        public VendorCriteria Criteria { get; }

        #endregion Properties

        #region ICommand

        public string EventName => "Insert Vendor";

        #endregion ICommand
    }
}
