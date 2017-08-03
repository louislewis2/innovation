namespace Innovation.Sample.Api.Customers.Commands
{
    using System.ComponentModel.DataAnnotations;

    using Innovation.Api.Commanding;

    public class CreateCustomer : ICommand
    {
        #region Properties

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username needs to be between 3 and 30 characters")]
        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        #endregion Properties

        #region ICommand

        public string EventName => "Create Customer";

        #endregion ICommand
    }
}
