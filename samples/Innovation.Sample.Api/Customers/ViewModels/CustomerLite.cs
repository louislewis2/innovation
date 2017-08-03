namespace Innovation.Sample.Api.Customers.ViewModels
{
    using System;

    using Innovation.Api.Querying;

    public class CustomerLite : IQueryResult
    {
        #region Constructor

        public CustomerLite(Guid id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        #endregion Constructor

        #region Properties

        public Guid Id { get; private set; }
        public string FullName { get; private set; }

        #endregion Properties
    }
}
