namespace Innovation.ApiSample.Customers.Commands
{
    using System.ComponentModel.DataAnnotations;

    using Innovation.Api.Commanding;

    using Criteria;

    public class InsertCustomer : ICommand
    {
        #region Constructor

        public InsertCustomer(CustomerCriteria customerCriteria)
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
