namespace Innovation.Sample.Data.Anemics.Customers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Innovation.Sample.Api.Customers.Statics;

    [Table("Customers")]
    public class CustomerAnemic : AnemicBase
    {
        #region Constructor

        public CustomerAnemic(
            Guid id,
            string userName,
            string firstName,
            string lastName,
            string email,
            string phoneNumber) : base(id)
        {
            this.UserName = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }

        #endregion Constructor

        #region Properties

        [Required(ErrorMessage = CustomerCriteriaStatics.UserNameRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.UserNameMinLength, ErrorMessage = CustomerCriteriaStatics.UserNameMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.UserNameMaxLength, ErrorMessage = CustomerCriteriaStatics.UserNameMaxErrorMessage)]
        public string UserName { get; set; }

        [Required(ErrorMessage = CustomerCriteriaStatics.FirstNameRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.FirstNameMinLength, ErrorMessage = CustomerCriteriaStatics.FirstNameMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.FirstNameMaxLength, ErrorMessage = CustomerCriteriaStatics.FirstNameMaxErrorMessage)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = CustomerCriteriaStatics.LastNameRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.LastNameMinLength, ErrorMessage = CustomerCriteriaStatics.LastNameMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.LastNameMaxLength, ErrorMessage = CustomerCriteriaStatics.LastNameMaxErrorMessage)]
        public string LastName { get; set; }

        [Required(ErrorMessage = CustomerCriteriaStatics.EmailRequiredErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.FirstNameMinLength, ErrorMessage = CustomerCriteriaStatics.EmailMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.FirstNameMaxLength, ErrorMessage = CustomerCriteriaStatics.EmailMaxErrorMessage)]
        public string Email { get; set; }

        [Required(ErrorMessage = CustomerCriteriaStatics.PhoneNumberRequiredErrorMessage)]
        [RegularExpression(pattern: CustomerCriteriaStatics.PhoneNumberRegularExpression, ErrorMessage = CustomerCriteriaStatics.PhoneNumberRegularExpressionErrorMessage)]
        [MinLength(length: CustomerCriteriaStatics.PhoneNumberMinLength, ErrorMessage = CustomerCriteriaStatics.PhoneNumberMinErrorMessage)]
        [MaxLength(length: CustomerCriteriaStatics.PhoneNumberMaxLength, ErrorMessage = CustomerCriteriaStatics.PhoneNumberMaxErrorMessage)]
        public string PhoneNumber { get; set; }

        #endregion Properties

        #region Methods

        public static CustomerAnemic New(
            Guid id,
            string userName,
            string firstName,
            string lastName,
            string email,
            string phoneNumber)
        {
            return new CustomerAnemic(
                id: id,
                userName: userName,
                firstName: firstName,
                lastName: lastName,
                email: email,
                phoneNumber: phoneNumber);
        }

        public string GetFullName()
        {
            return $"{this.FirstName} - {this.LastName}";
        }

        #endregion Methods
    }
}
