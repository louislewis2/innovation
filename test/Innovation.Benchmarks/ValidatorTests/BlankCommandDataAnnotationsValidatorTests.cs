namespace Innovation.Benchmarks.ValidatorTests
{
    using System;
    using System.Threading.Tasks;

    using MiniValidation;
    using BenchmarkDotNet.Attributes;
    using Innovation.ServiceBus.InProcess.Validators;

    using Innovation.ApiSample;

    [MemoryDiagnoser]
    public class BlankCommandDataAnnotationsValidatorTests : DependencyBuilderBase
    {
        #region Fields

        private IServiceProvider serviceProvider;
        private DataAnnotationsValidator dataAnnotationsValidatorNew;
        private static BlankCommand blankCommand = new BlankCommand();

        #endregion Fields

        #region Methods

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.serviceProvider = this.GetRequiredService<IServiceProvider>();
            this.dataAnnotationsValidatorNew = new DataAnnotationsValidator(serviceProvider: serviceProvider);

            // This is to warmup the type cache, to make the results comparible with the previous implementation
            MiniValidator.TryValidate(target: blankCommand, this.serviceProvider, out var errors);
        }

        [Benchmark]
        public async ValueTask<bool> BlankCommandNew()
        {
            var validationResult = await dataAnnotationsValidatorNew.TryValidateObjectRecursive(target: blankCommand);

            return validationResult.isValid;
        }

        #endregion Methods
    }
}
