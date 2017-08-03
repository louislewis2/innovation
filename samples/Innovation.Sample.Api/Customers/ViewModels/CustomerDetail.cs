namespace Innovation.Sample.Api.Customers.ViewModels
{
    using System;

    using Innovation.Api.Querying;

    public class CustomerDetail : IQueryResult
    {
        #region Properties

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        #endregion Properties
    }
}
