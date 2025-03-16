namespace Innovation.ApiSample.Customers.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Innovation.Api.Core;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;

    using Criteria;
    using Innovation.ApiSample.Shared.Contexts;

    public class UpdateCustomerCommand : ICommand, IContextAware
    {
        #region Constructor

        public UpdateCustomerCommand(CustomerCriteria customerCriteria, Guid customerId)
        {
            this.Criteria = customerCriteria;
            this.CustomerId = customerId;
        }

        #endregion Constructor

        #region Properties

        [Required]
        public Guid CustomerId { get; }

        [Required]
        public CustomerCriteria Criteria { get; }

        public SharedDispatcherContext DispatcherContext { get; private set; }

        #endregion Properties

        #region ICommand

        public string EventName => "Insert Customer";

        public void SetContext(IDispatcherContext dispatcherContext)
        {
            this.DispatcherContext = dispatcherContext as SharedDispatcherContext;
        }

        #endregion ICommand
    }
}
