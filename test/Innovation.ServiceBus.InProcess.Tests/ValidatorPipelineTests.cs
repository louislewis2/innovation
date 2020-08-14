namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;

    using Xunit;
    using Innovation.Api.Commanding;

    using ApiSample.Validation;
    using ApiSample.Vendors.Criteria;
    using ApiSample.Vendors.Commands;
    using ApiSample.Suppliers.Criteria;
    using ApiSample.Suppliers.Commands;
    using Innovation.ApiSample.Shared.Criteria;

    public class ValidatorPipelineTests : TestBase
    {
        #region Methods

        [Fact]
        public async Task Can_Receive_ValidationResult()
        {
            // Arrange
            var supplierCriteria = new SupplierCriteria(name: "CoolName");
            var insertSupplierCommand = new InsertSupplier(supplierCriteria: supplierCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertSupplierCommand)).As<SampleValidationResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: "Another Test Error Message", actual: commandResult.Errors[0].ErrorMessage);
        }

        [Fact]
        public async Task Can_Invoke_Command_Validator()
        {
            // Arrange
            // Arrange
            var vendorCriteria = new VendorCriteria(
                name: "Innovation",
                userName: "SomeUserNameThatRocks",
                address: new AddressCriteria(line1: "111 Street", code: null));
            var insertVendorCommand = new InsertVendor(vendorCriteria: vendorCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertVendorCommand)).As<SampleValidationResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: "Street Cannot Be 111 Street", actual: commandResult.Errors[0].ErrorMessage);
        }

        #endregion Methods
    }
}
