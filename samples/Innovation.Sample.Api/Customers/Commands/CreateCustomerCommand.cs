namespace Innovation.Sample.Api.Customers.Commands
{
    using Innovation.Api.Commanding;
    using Innovation.Sample.Api.Customers.Criteria;

    public class CreateCustomerCommand : ICommand
    {
        #region Constructor

        public CreateCustomerCommand(CreateCustomerCriteria createCustomerCriteria)
        {
            this.Criteria = createCustomerCriteria;
        }

        #endregion Constructor

        #region Properties

        public CreateCustomerCriteria Criteria { get; }

        #endregion Properties

        #region ICommand

        public string EventName => "Create Customer";

        #endregion ICommand
    }
}
