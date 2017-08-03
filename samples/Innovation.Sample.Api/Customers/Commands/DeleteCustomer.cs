namespace Innovation.Sample.Api.Customers.Commands
{
    using System;

    using Innovation.Api.Commanding;

    public class DeleteCustomer : ICommand
    {
        #region Constructor

        public DeleteCustomer(Guid id)
        {
            Id = id;
        }

        #endregion Constructor

        #region Properties

        public Guid Id { get; private set; }

        #endregion Properties

        #region ICommand

        public string EventName => "Delete Customer";

        #endregion ICommand
    }
}
