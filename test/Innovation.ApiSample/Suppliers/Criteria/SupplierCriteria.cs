namespace Innovation.ApiSample.Suppliers.Criteria
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class SupplierCriteria : IValidatableObject
    {
        #region Constructor

        public SupplierCriteria(string name)
        {
            this.Name = name;
        }

        #endregion Constructor

        #region Properties

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name needs to be between 3 and 30 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Name only allows alphanumeric characters")]
        public string Name { get; set; }

        #endregion Properties

        #region Methods

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (!string.IsNullOrWhiteSpace(value: this.Name))
            {
                if (this.Name == "Louis")
                {
                    validationResults.Add(new ValidationResult("Name Cannot Be Louis", [nameof(Name)]));
                }
            }

            return validationResults;
        }

        #endregion Methods
    }
}
