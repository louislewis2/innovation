namespace Innovation.SampleApi.Consumer.Handlers.Suppliers.CommandValidators
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using System.ComponentModel.DataAnnotations;

    using Innovation.Api.Commanding;

    using Innovation.SampleApi.Consumer;
    using Innovation.ApiSample.Validation;
    using Innovation.ApiSample.Suppliers.Commands;

    public class InsertSupplierPersistor : PersistorBase<InsertSupplierCommand>
    {
        #region Constructor

        public InsertSupplierPersistor(ILogger<InsertSupplierPersistor> logger) : base(logger: logger)
        {
        }

        #endregion Constructor

        #region Methods

        public override Task<ICommandResult> Persist()
        {
            var sampleValidationResult = new SampleValidationResult();

            sampleValidationResult.Add(validationResult: new ValidationResult(errorMessage: "Another Test Error Message"));

            return Task.FromResult((ICommandResult)sampleValidationResult);
        }

        #endregion Methods
    }
}
