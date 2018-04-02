namespace Innovation.ApiSample.Customers.Commands
{
    using System.ComponentModel.DataAnnotations;

    using Api.Commanding;

    public class InsertCustomer : ICommand
    {
        #region Constructor

        public InsertCustomer(string name, string userName)
        {
            this.Name = name;
            this.UserName = userName;

            this.ExistsOnGithub = null;
        }

        #endregion Constructor

        #region Properties

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name needs to be between 3 and 30 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Name only allows alphanumeric characters")]
        public string Name { get; private set; }

        [Required]
        [StringLength(35, MinimumLength = 10, ErrorMessage = "UserName needs to be between 10 and 35 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "UserName only allows alphanumeric characters")]
        public string UserName { get; private set; }

        public bool? ExistsOnGithub { get; private set; }

        #endregion Properties

        #region Methods

        public void SetGithubStatus(bool existsOnGithub)
        {
            this.ExistsOnGithub = existsOnGithub;
        }

        #endregion Methods

        #region ICommand

        public string EventName => "Insert Customer";

        #endregion ICommand
    }
}
