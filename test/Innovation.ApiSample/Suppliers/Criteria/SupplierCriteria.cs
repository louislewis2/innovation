namespace Innovation.ApiSample.Suppliers.Criteria
{
    using System.ComponentModel.DataAnnotations;

    public class SupplierCriteria
    {
        #region Properties

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name needs to be between 3 and 30 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Name only allows alphanumeric characters")]
        public string Name { get; set; }

        #endregion Properties
    }
}
