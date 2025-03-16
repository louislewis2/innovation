namespace Innovation.ApiSample.Customers.Commands
{
    using Innovation.Api.Commanding;

    using Criteria;

    public class InsertCustomerCommand : ICommand
    {
        #region Constructor

        public InsertCustomerCommand(CustomerCriteria customerCriteria)
        {
            this.Criteria = customerCriteria;
        }

        #endregion Constructor

        #region Properties

        public CustomerCriteria Criteria { get; }

        #endregion Properties

        #region ICommand

        public string EventName => "Insert Customer";

        #endregion ICommand
    }
}
