namespace Innovation.Sample.Api.Customers.Criteria
{
    using System.ComponentModel.DataAnnotations;

    using Innovation.Sample.Api.Customers.Statics;

    public class CreateCustomerCriteria
    {
        #region Constructor

        // MVC Requires a parameterless constructor
        public CreateCustomerCriteria()
        {
        }

        public CreateCustomerCriteria(
            string userName,
            string firstName,
            string lastName,
            string email,
            string phoneNumber)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }

        #endregion Constructor

        #region Properties

        // MVC Requires setters to be present
        // For rest api's we do not need setters

        /// <summary>
        /// The GitHub username for the customer
        /// </summary>
        /// <example>louislewis2</example>
        [Required(ErrorMessage = CustomerCriteriaStatics.UserNameRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.UserNameMinLength, ErrorMessage = CustomerCriteriaStatics.UserNameMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.UserNameMaxLength, ErrorMessage = CustomerCriteriaStatics.UserNameMaxErrorMessage)]
        public string UserName { get; set; }

        /// <summary>
        /// The first name of the customer
        /// </summary>
        /// <example>Louis</example>
        [Required(ErrorMessage = CustomerCriteriaStatics.FirstNameRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.FirstNameMinLength, ErrorMessage = CustomerCriteriaStatics.FirstNameMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.FirstNameMaxLength, ErrorMessage = CustomerCriteriaStatics.FirstNameMaxErrorMessage)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the customer
        /// </summary>
        /// <example>Lewis</example>
        [Required(ErrorMessage = CustomerCriteriaStatics.LastNameRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.LastNameMinLength, ErrorMessage = CustomerCriteriaStatics.LastNameMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.LastNameMaxLength, ErrorMessage = CustomerCriteriaStatics.LastNameMaxErrorMessage)]
        public string LastName { get; set; }

        /// <summary>
        /// The email address for the customer
        /// </summary>
        /// <example>doesnotexist@nopcxxttaw.for</example>
        [Required(ErrorMessage = CustomerCriteriaStatics.EmailRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.EmailMinLength, ErrorMessage = CustomerCriteriaStatics.EmailMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.EmailMaxLength, ErrorMessage = CustomerCriteriaStatics.EmailMaxErrorMessage)]
        public string Email { get; set; }

        /// <summary>
        /// The phone numner for the customer
        /// </summary>
        /// <example>+27 87 5555555</example>
        [Required(ErrorMessage = CustomerCriteriaStatics.PhoneNumberRequiredErrorMessage)]
        [RegularExpression(pattern: CustomerCriteriaStatics.PhoneNumberRegularExpression, ErrorMessage = CustomerCriteriaStatics.PhoneNumberRegularExpressionErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.PhoneNumberMinLength, ErrorMessage = CustomerCriteriaStatics.PhoneNumberMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.PhoneNumberMaxLength, ErrorMessage = CustomerCriteriaStatics.PhoneNumberMaxErrorMessage)]
        public string PhoneNumber { get; set; }

        #endregion Properties

        #region Methods

        public static CreateCustomerCriteria Default()
        {
            return new CreateCustomerCriteria(
                userName: null,
                firstName: null,
                lastName: null,
                email: null,
                phoneNumber: null);
        }

        #endregion Methods
    }
}
