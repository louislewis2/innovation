namespace Innovation.Sample.Api.Customers.Queries
{
    using System;

    using Innovation.Api.Querying;

    public class GetCustomer : IQuery
    {
        #region Constructor

        public GetCustomer(Guid id)
        {
            this.Id = id;
        }

        #endregion Constructor

        #region Properties

        public Guid Id { get; set; }

        #endregion Properties

        #region IQuery

        public string EventName => "Retrieve a single customer";

        #endregion IQuery
    }
}
