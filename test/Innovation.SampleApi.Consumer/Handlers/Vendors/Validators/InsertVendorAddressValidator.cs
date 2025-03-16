namespace Innovation.SampleApi.Consumer.Handlers.Vendors.Validators
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;

    using Innovation.Api.Validation;

    using ApiSample.Validation;
    using ApiSample.Vendors.Commands;

    public class InsertVendorAddressValidator : IValidator<InsertVendorCommand>
    {
        #region Fields

        private readonly ILogger logger;

        #endregion Fields

        #region Constructor

        public InsertVendorAddressValidator(ILogger<InsertVendorAddressValidator> logger)
        {
            this.logger = logger;
        }

        #endregion Constructor

        #region Methods

        public async Task<IValidationResult> Validate(InsertVendorCommand command)
        {
            var sampleValidationResult = new SampleValidationResult();

            if (command.Criteria == null)
            {
                sampleValidationResult.Add(validationResult: new ValidationResult(errorMessage: "Cannot be null", memberNames: new[] { nameof(command.Criteria) }));
            }
            else if (command.Criteria.Address == null)
            {
                sampleValidationResult.Add(validationResult: new ValidationResult(errorMessage: "Cannot be null", memberNames: new[] { nameof(command.Criteria.Address) }));
            }
            else
            {
                if (command.Criteria.Address.Line1 == "111 Street")
                {
                    sampleValidationResult.Add(validationResult: new ValidationResult(errorMessage: "Street Cannot Be 111 Street", memberNames: new[] { nameof(command.Criteria.Address.Line1) }));
                }
            }

            return await Task.FromResult(sampleValidationResult);
        }

        #endregion Methods
    }
}
