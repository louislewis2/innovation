namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Vendors.Criteria;
    using ApiSample.Vendors.Commands;
    using ApiSample.Customers.Criteria;
    using ApiSample.Customers.Commands;

    public class CommandTests : TestBase
    {
        [Fact]
        public void Can_Handle_Insert_Customer()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "somecrazynamethatdoesnotexistyet");
            var insertCustomerCommand = new InsertCustomer(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var canCommand = dispatcher.CanCommand(command: insertCustomerCommand);

            // Assert
            Assert.True(condition: canCommand);
        }

        [Fact]
        public async Task Can_Insert_Customer()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "somecrazynamethatdoesnotexistyet");
            var insertCustomerCommand = new InsertCustomer(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(command: insertCustomerCommand, suppressExceptions: false);

            // Assert
            Assert.True(condition: insertCustomerCommandResult.Success);
        }

        [Fact]
        public async Task Can_Insert_Vendor()
        {
            // Arrange
            var vendorCriteria = new VendorCriteria(
                name: "Innovation",
                userName: "SomeUserNameThatRocks",
                address: null);
            var insertVendorCommand = new InsertVendor(vendorCriteria: vendorCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var insertVendorCommandResult = await dispatcher.Command(command: insertVendorCommand, suppressExceptions: false);

            // Assert
            Assert.True(condition: insertVendorCommandResult.Success);
            Assert.IsType<CommandResult>(@object: insertVendorCommandResult);
        }
    }
}
