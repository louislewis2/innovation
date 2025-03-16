namespace Innovation.Sample.Api.Customers.Criteria
{
    using System.ComponentModel.DataAnnotations;

    using Innovation.Sample.Api.Customers.Statics;

    public class UpdateCustomerCriteria
    {
        #region Constructor

        // MVC Requires a parameterless constructor
        public UpdateCustomerCriteria()
        {
        }

        public UpdateCustomerCriteria(string firstName, string lastName, string phoneNumber, string email)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
        }

        #endregion Constructor

        #region Properties

        [Required(ErrorMessage = CustomerCriteriaStatics.FirstNameRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.FirstNameMinLength, ErrorMessage = CustomerCriteriaStatics.FirstNameMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.FirstNameMaxLength, ErrorMessage = CustomerCriteriaStatics.FirstNameMaxErrorMessage)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = CustomerCriteriaStatics.LastNameRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.LastNameMinLength, ErrorMessage = CustomerCriteriaStatics.LastNameMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.LastNameMaxLength, ErrorMessage = CustomerCriteriaStatics.LastNameMaxErrorMessage)]
        public string LastName { get; set; }

        [Required(ErrorMessage = CustomerCriteriaStatics.EmailRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.EmailMinLength, ErrorMessage = CustomerCriteriaStatics.EmailMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.EmailMaxLength, ErrorMessage = CustomerCriteriaStatics.EmailMaxErrorMessage)]
        public string Email { get; set; }

        [Required(ErrorMessage = CustomerCriteriaStatics.PhoneNumberRequiredErrorMessage)]
        [RegularExpression(pattern: CustomerCriteriaStatics.PhoneNumberRegularExpression, ErrorMessage = CustomerCriteriaStatics.PhoneNumberRegularExpressionErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.PhoneNumberMinLength, ErrorMessage = CustomerCriteriaStatics.PhoneNumberMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.PhoneNumberMaxLength, ErrorMessage = CustomerCriteriaStatics.PhoneNumberMaxErrorMessage)]
        public string PhoneNumber { get; set; }

        #endregion Properties

        #region Methods

        public static UpdateCustomerCriteria Default()
        {
            return new UpdateCustomerCriteria(
                firstName:  null,
                lastName: null,
                phoneNumber: null,
                email: null);
        }

        #endregion Methods
    }
}
