namespace Innovation.Sample.Api.Customers.Commands
{
    using System;

    using Innovation.Api.Commanding;

    public class UpdateCustomer : ICommand
    {
        #region Properties

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        #endregion Properties

        #region ICommand

        public string EventName => "Update Customer";

        #endregion ICommand
    }
}
