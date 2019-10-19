namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;

    using Xunit;
    using Innovation.Api.Commanding;

    using Innovation.ApiSample.Validation;
    using Innovation.ApiSample.Suppliers.Criteria;
    using Innovation.ApiSample.Suppliers.Commands;

    public class ValidatorPipelineTests : TestBase
    {
        #region Methods

        [Fact]
        public async Task Can_Receive_ValidationResult()
        {
            // Arrange
            var supplierCriteria = new SupplierCriteria { Name = "Some Valid Name" };
            var insertSupplierCommand = new InsertSupplier(criteria: supplierCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertSupplierCommand)).As<SampleValidationResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: "Another Test Error Message", actual: commandResult.Errors[0].ErrorMessage);
        }

        #endregion Methods
    }
}
