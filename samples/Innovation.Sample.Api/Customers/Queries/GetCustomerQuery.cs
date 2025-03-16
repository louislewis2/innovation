namespace Innovation.Sample.Api.Customers.Queries
{
    using System;

    using Innovation.Api.Querying;

    public class GetCustomerQuery : IQuery
    {
        #region Constructor

        public GetCustomerQuery(Guid customerId)
        {
            this.CustomerId = customerId;
        }

        #endregion Constructor

        #region Properties

        public Guid CustomerId { get; }

        #endregion Properties

        #region IQuery

        public string EventName => nameof(GetCustomerQuery);

        #endregion IQuery
    }
}
