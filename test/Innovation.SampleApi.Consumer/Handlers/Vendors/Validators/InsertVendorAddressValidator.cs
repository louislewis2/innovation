namespace Innovation.SampleApi.Consumer.Handlers.Vendors.Validators
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;

    using Innovation.Api.Validation;

    using ApiSample.Validation;
    using ApiSample.Vendors.Commands;

    public class InsertVendorAddressValidator : IValidator<InsertVendor>
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

        public async Task<IValidationResult> Validate(InsertVendor command)
        {
            var sampleValidationResult = new SampleValidationResult();

            if (command.Criteria?.Address != null)
            {
                if (command.Criteria.Address.Line1 == "111 Street")
                {
                    sampleValidationResult.Add(new ValidationResult(errorMessage: "Street Cannot Be 111 Street", memberNames: new[] { nameof(command.Criteria.Address.Line1) }));
                }
            }

            return await Task.FromResult(sampleValidationResult);
        }

        #endregion Methods
    }
}
