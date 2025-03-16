namespace Innovation.ServiceBus.InProcess.Validators
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MiniValidation;

    public class DataAnnotationsValidator
    {
        #region Fields

        private readonly IServiceProvider serviceProvider;

        #endregion Fields

        #region Constructor

        public DataAnnotationsValidator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        #endregion Constructor

        #region Methods

        public async ValueTask<(bool isValid, IDictionary<string, string[]> Errors)> TryValidateObject<TTarget>(TTarget target)
        {
            return await MiniValidator.TryValidateAsync(target, this.serviceProvider, false);
        }

        public async ValueTask<(bool isValid, IDictionary<string, string[]> Errors)> TryValidateObjectRecursive<TTarget>(TTarget target)
        {
            return await MiniValidator.TryValidateAsync(target, this.serviceProvider, true);
        }

        #endregion Methods

        #region Methods

        private async ValueTask<(bool IsValid, IDictionary<string, string[]> Errors)> TryValidateObjectImplementationNew<TTarget>(TTarget target, bool recursive)
        {
            return await MiniValidator.TryValidateAsync(target, this.serviceProvider, recursive);
        }

        private async ValueTask<(bool isValid, ValidationResult[] validationResults)> TryValidateObjectImplementation<TTarget>(TTarget target, bool recursive)
        {
            var validationResult = await MiniValidator.TryValidateAsync(target, this.serviceProvider, recursive);

            var validatorResults = validationResult.Errors.ConvertToValidationResult();

            return (validationResult.IsValid, validatorResults);
        }

        #endregion Methods
    }
}
