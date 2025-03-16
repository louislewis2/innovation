namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Innovation.Api.Commanding;

    using ApiSample.Validation;
    using ApiSample.Shared.Criteria;
    using ApiSample.Vendors.Criteria;
    using ApiSample.Vendors.Commands;
    using ApiSample.Suppliers.Criteria;
    using ApiSample.Suppliers.Commands;

    [TestClass]
    public class ValidatorPipelineTests : TestBase
    {
        #region Methods

        [TestMethod]
        public async Task Can_Receive_ValidationResult()
        {
            // Arrange
            var supplierCriteria = new SupplierCriteria(name: "CoolName");
            var insertSupplierCommand = new InsertSupplierCommand(supplierCriteria: supplierCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertSupplierCommand)).As<SampleValidationResult>();

            // Assert
            Assert.IsFalse(condition: commandResult.Success);
            Assert.AreEqual(expected: "Another Test Error Message", actual: commandResult.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task Can_Invoke_Command_Validator()
        {
            // Arrange
            // Arrange
            var vendorCriteria = new VendorCriteria(
                name: "Innovation",
                userName: "SomeUserNameThatRocks",
                addressCriteria: new AddressCriteria(line1: "111 Street", code: null));
            var insertVendorCommand = new InsertVendorCommand(vendorCriteria: vendorCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertVendorCommand)).As<SampleValidationResult>();

            // Assert
            Assert.IsFalse(condition: commandResult.Success);
            Assert.AreEqual(expected: "Street Cannot Be 111 Street", actual: commandResult.Errors[0].ErrorMessage);
        }

        #endregion Methods
    }
}
