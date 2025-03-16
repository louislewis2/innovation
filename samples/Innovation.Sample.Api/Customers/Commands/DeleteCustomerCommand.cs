namespace Innovation.Sample.Api.Customers.Commands
{
    using System;

    using Innovation.Api.Commanding;

    public class DeleteCustomerCommand : ICommand
    {
        #region Constructor

        public DeleteCustomerCommand(Guid customerId)
        {
            this.CustomerId = customerId;
        }

        #endregion Constructor

        #region Properties

        public Guid CustomerId { get; }

        #endregion Properties

        #region ICommand

        public string EventName => "Delete Customer";

        #endregion ICommand
    }
}
