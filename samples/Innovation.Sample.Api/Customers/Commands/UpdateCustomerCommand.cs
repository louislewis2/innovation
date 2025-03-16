namespace Innovation.Sample.Api.Customers.Commands
{
    using System;

    using Innovation.Api.Commanding;

    using Innovation.Sample.Api.Customers.Criteria;

    public class UpdateCustomerCommand : ICommand
    {
        #region Constructor

        public UpdateCustomerCommand(Guid customerId, UpdateCustomerCriteria updateCustomerCriteria)
        {
            this.CustomerId = customerId;
            this.Criteria = updateCustomerCriteria;
        }

        #endregion Constructor

        #region Properties

        public Guid CustomerId { get; }
        public UpdateCustomerCriteria Criteria { get; }

        #endregion Properties

        #region ICommand

        public string EventName => "Update Customer";

        #endregion ICommand
    }
}
