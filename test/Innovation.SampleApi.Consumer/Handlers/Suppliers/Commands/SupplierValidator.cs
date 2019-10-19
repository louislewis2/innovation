namespace Innovation.SampleApi.Consumer.Handlers.Suppliers.Commands
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;

    using Innovation.ApiSample.Validation;
    using Innovation.ApiSample.Suppliers.Commands;
    using System.ComponentModel.DataAnnotations;

    public class SupplierValidator : PersistorBase<InsertSupplier>
    {
        #region Constructor

        public SupplierValidator(ILogger<SupplierValidator> logger) : base(logger: logger)
        {
        }

        #endregion Constructor

        #region Methods

        public override Task<ICommandResult> Persist()
        {
            var sampleValidationResult = new SampleValidationResult();

            sampleValidationResult.Add(new ValidationResult(errorMessage: "Another Test Error Message"));

            return Task.FromResult((ICommandResult)sampleValidationResult);
        }

        #endregion Methods
    }
}
