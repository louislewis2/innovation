namespace Innovation.Sample.Api.Customers.ViewModels
{
    using System;

    using Innovation.Api.Querying;

    public class CustomerDetail : IQueryResult
    {
        #region Constructor

        public CustomerDetail(
            Guid id,
            string firstName,
            string lastName,
            string phoneNumber,
            string email)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// The unigue identifer for this customer.
        /// It is also referred to customerId in other api calls
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The first name of the customer
        /// </summary>
        /// <example>Louis</example>
        public string FirstName { get; }

        /// <summary>
        /// The last name of the customer
        /// </summary>
        /// <example>Lewis</example>
        public string LastName { get; }

        /// <summary>
        /// The phone numner for the customer
        /// </summary>
        /// <example>+27 87 5555555</example>
        public string PhoneNumber { get; }

        /// <summary>
        /// The email address for the customer
        /// </summary>
        /// <example>doesnotexist@nopcxxttaw.for</example>
        public string Email { get; }

        #endregion Properties
    }
}
