namespace Innovation.ApiSample.Vendors.Criteria
{
    using System.ComponentModel.DataAnnotations;

    using Shared.Criteria;

    public class VendorCriteria
    {
        #region Constructor

        public VendorCriteria(string name, string userName, AddressCriteria address)
        {
            this.Name = name;
            this.UserName = userName;
            this.Address = address;
        }

        #endregion Constructor

        #region Properties

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name needs to be between 3 and 30 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Name only allows alphanumeric characters")]
        public string Name { get; }

        [Required]
        [StringLength(35, MinimumLength = 10, ErrorMessage = "UserName needs to be between 10 and 35 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "UserName only allows alphanumeric characters")]
        public string UserName { get; }

        public AddressCriteria Address { get; }

        #endregion Properties
    }
}
