namespace Innovation.Benchmarks.ValidatorTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BenchmarkDotNet.Attributes;
    using Innovation.ServiceBus.InProcess.Validators;

    using Innovation.ApiSample.Customers.Commands;
    using Innovation.ApiSample.Customers.Criteria;

    /// <summary>
    /// A benchmark class to test the performance of the DataAnnotationsValidator with a specific command object (InsertCustomer).
    /// </summary>
    [MemoryDiagnoser]
    public class DataAnnotationsValidatorTests : DependencyBuilderBase
    {
        #region Fields

        private IServiceProvider serviceProvider;
        private DataAnnotationsValidator dataAnnotationsValidatorNew;
        private static CustomerCriteria customerCriteria = new CustomerCriteria(
            name: "Louis",
            userName: "louislewis2");
        private static InsertCustomer insertCustomer = new InsertCustomer(customerCriteria: customerCriteria);
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
        public bool InsertCustomerCommand()
        {
            var validationResult =  dataAnnotationsValidatorNew.TryValidateObjectRecursive(obj: insertCustomer, results: validationResults);

            return validationResult;
        }

        #endregion Methods
    }
}
