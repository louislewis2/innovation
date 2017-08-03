namespace Innovation.ApiSample.Customers.Commands
{
    using System.ComponentModel.DataAnnotations;

    using Api.Commanding;

    public class InsertCustomer : ICommand
    {
        #region Constructor

        public InsertCustomer(string name)
        {
            this.Name = name;
        }

        #endregion Constructor

        #region Properties

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name needs to be between 3 and 30 characters")]
        public string Name { get; private set; }

        #endregion Properties

        #region ICommand

        public string EventName => "Insert Customer";

        #endregion ICommand
    }
}
