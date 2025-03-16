namespace Innovation.Sample.Api.Customers.Statics
{
    public static class CustomerCriteriaStatics
    {
        // UserName
        public const string UserNameRequiredErrorMessage = "Is mandatory";

        public const int UserNameMinLength = 3;
        public const string UserNameMinErrorMessage = "Must be at least 3 characters";

        public const int UserNameMaxLength = 30;
        public const string UserNameMaxErrorMessage = "Cannot exceed 30 characters";

        // FirstName

        public const string FirstNameRequiredErrorMessage = "Is mandatory";

        public const int FirstNameMinLength = 3;
        public const string FirstNameMinErrorMessage = "Must be at least 3 characters";

        public const int FirstNameMaxLength = 30;
        public const string FirstNameMaxErrorMessage = "Cannot exceed 30 characters";

        // LastName
        public const string LastNameRequiredErrorMessage = "Is mandatory";

        public const int LastNameMinLength = 3;
        public const string LastNameMinErrorMessage = "Must be at least 3 characters";

        public const int LastNameMaxLength = 30;
        public const string LastNameMaxErrorMessage = "Cannot exceed 30 characters";

        // Email
        public const string EmailRequiredErrorMessage = "Is mandatory";

        public const int EmailMinLength = 3;
        public const string EmailMinErrorMessage = "Must be at least 3 characters";

        public const int EmailMaxLength = 30;
        public const string EmailMaxErrorMessage = "Cannot exceed 30 characters";

        // PhoneNumber
        public const string PhoneNumberRequiredErrorMessage = "Is mandatory";

        public const string PhoneNumberRegularExpression = @"^\+[0-9]{2}\s+[0-9]{2}\s+[0-9]{7}$";
        public const string PhoneNumberRegularExpressionErrorMessage = "Incorrect format";

        public const int PhoneNumberMinLength = 10;
        public const string PhoneNumberMinErrorMessage = "Must be at least 10 characters";

        public const int PhoneNumberMaxLength = 15;
        public const string PhoneNumberMaxErrorMessage = "Cannot exceed 15 characters";
    }
}
