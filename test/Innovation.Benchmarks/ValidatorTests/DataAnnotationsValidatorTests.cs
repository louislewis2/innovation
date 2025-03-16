namespace Innovation.Benchmarks.ValidatorTests
{
    using System;
    using System.Threading.Tasks;

    using MiniValidation;
    using BenchmarkDotNet.Attributes;
    using Innovation.ServiceBus.InProcess.Validators;

    using Innovation.ApiSample.Customers.Commands;
    using Innovation.ApiSample.Customers.Criteria;

    [MemoryDiagnoser]
    public class DataAnnotationsValidatorTests : DependencyBuilderBase
    {
        #region Fields

        private IServiceProvider serviceProvider;
        private DataAnnotationsValidator dataAnnotationsValidatorNew;
        private static CustomerCriteria customerCriteria = new CustomerCriteria(
            name: "Louis",
            userName: "louislewis2");
        private static InsertCustomerCommand insertCustomer = new InsertCustomerCommand(customerCriteria: customerCriteria);

        #endregion Fields

        #region Methods

        [GlobalSetup]
        public void GlobalSetup()
        {
            this.serviceProvider = this.GetRequiredService<IServiceProvider>();
            this.dataAnnotationsValidatorNew = new DataAnnotationsValidator(serviceProvider: serviceProvider);

            // This is to warmup the type cache, to make the results comparible with the previous implementation
            MiniValidator.TryValidate(target: insertCustomer, this.serviceProvider, out var errors);
        }

        [Benchmark]
        public async Task<bool> TestNew()
        {
            var validationResult =  await dataAnnotationsValidatorNew.TryValidateObjectRecursive(target: insertCustomer);

            return validationResult.isValid;
        }

        #endregion Methods
    }
}
