namespace Innovation.ApiSample.Vendors.Commands
{
    using System.ComponentModel.DataAnnotations;

    using Innovation.Api.Dispatching;

    using Api.Commanding;

    public class InsertVendor : ICommand, IDispatcherContext
    {
        #region Constructor

        public InsertVendor(string name, string userName)
        {
            this.Name = name;
            this.UserName = userName;
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

        public object DispatcherContext { get; set; }
        public string CorrelationId { get; set; }

        #endregion Properties

        #region ICommand

        public string EventName => "Insert Vendor";

        #endregion ICommand
    }
}
