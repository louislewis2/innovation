namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Innovation.Api.CommandHelpers;

    using ApiSample;
    using ApiSample.Shared.Criteria;
    using ApiSample.Vendors.Criteria;
    using ApiSample.Vendors.Commands;
    using ApiSample.Customers.Criteria;
    using ApiSample.Customers.Commands;

    [TestClass]
    public class CommandTests : TestBase
    {
        [TestMethod]
        public async Task Can_Insert_Customer()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "somecrazynamethatdoesnotexistyet");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(command: insertCustomerCommand, suppressExceptions: false);

            // Assert
            Assert.IsTrue(condition: insertCustomerCommandResult.Success);
        }

        [TestMethod]
        public async Task Can_Insert_Vendor()
        {
            // Arrange
            var addressCriteria = new AddressCriteria(
                line1: "1 Some street",
                code: "0001");

            var vendorCriteria = new VendorCriteria(
                name: "Innovation",
                userName: "SomeUserNameThatRocks",
                addressCriteria: addressCriteria);
            var insertVendorCommand = new InsertVendorCommand(vendorCriteria: vendorCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var insertVendorCommandResult = await dispatcher.Command(command: insertVendorCommand, suppressExceptions: false);

            // Assert
            Assert.IsTrue(condition: insertVendorCommandResult.Success);
            Assert.IsInstanceOfType<CommandResult>(value: insertVendorCommandResult);
        }

        [TestMethod]
        public async Task Can_Insert_BlankCommand()
        {
            // Arrange
            var insertBlankCommand = new BlankCommand();

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(command: insertBlankCommand, suppressExceptions: false);

            // Assert
            Assert.IsTrue(condition: insertCustomerCommandResult.Success);
        }
    }
}
