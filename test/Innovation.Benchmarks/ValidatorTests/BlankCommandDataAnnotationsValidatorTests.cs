namespace Innovation.Benchmarks.ValidatorTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BenchmarkDotNet.Attributes;
    using Innovation.ServiceBus.InProcess.Validators;

    using Innovation.ApiSample;

    /// <summary>
    /// A benchmark class to test the performance of the DataAnnotationsValidator with a BlankCommand.
    /// </summary>
    [MemoryDiagnoser]
    public class BlankCommandDataAnnotationsValidatorTests : DependencyBuilderBase
    {
        #region Fields

        private IServiceProvider serviceProvider;
        private DataAnnotationsValidator dataAnnotationsValidatorNew;
        private static BlankCommand blankCommand = new BlankCommand();
        private List<ValidationResult> validationResults;

        #endregion Fields

        #region Methods

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.serviceProvider = this.GetRequiredService<IServiceProvider>();
            this.dataAnnotationsValidatorNew = new DataAnnotationsValidator(serviceProvider: serviceProvider);
            this.validationResults = new List<ValidationResult>();
        }

        [Benchmark]
        public bool BlankCommandNew()
        {
            var validationResult = dataAnnotationsValidatorNew.TryValidateObjectRecursive(obj: blankCommand, results: validationResults);

            return validationResult;
        }

        #endregion Methods
    }
}
