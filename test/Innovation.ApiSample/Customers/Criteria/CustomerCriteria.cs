namespace Innovation.ApiSample.Customers.Criteria
{
    using System.ComponentModel.DataAnnotations;

    public class CustomerCriteria
    {
        #region Constructor

        public CustomerCriteria(string name, string userName)
        {
            this.Name = name;
            this.UserName = userName;
        }

        #endregion Constructor

        #region Properties

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "Name needs to be between 3 and 30 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Name only allows alphanumeric characters")]
        public string Name { get; }

        [Required]
        [StringLength(maximumLength: 35, MinimumLength = 10, ErrorMessage = "UserName needs to be between 10 and 35 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "UserName only allows alphanumeric characters")]
        public string UserName { get; }

        public bool? ExistsOnGithub { get; private set; }

        #endregion Properties

        #region Methods

        public void SetGithubStatus(bool existsOnGithub)
        {
            this.ExistsOnGithub = existsOnGithub;
        }

        #endregion Methods
    }
}
