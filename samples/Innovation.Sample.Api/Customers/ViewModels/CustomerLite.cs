namespace Innovation.Sample.Api.Customers.ViewModels
{
    using System;

    using Innovation.Api.Querying;

    public class CustomerLite : IQueryResult
    {
        #region Constructor

        public CustomerLite(Guid id, string fullName)
        {
            this.Id = id;
            this.FullName = fullName;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// The unigue identifer for this customer.
        /// It is also referred to customerId in other api calls
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// asdsad
        /// </summary>
        /// <example>Louis Lewis</example>
        public string FullName { get; }

        #endregion Properties
    }
}
