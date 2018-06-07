namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;
    using Innovation.Api.CommandHelpers;

    using Innovation.ApiSample.Vendors.Commands;
    using Innovation.ApiSample.Customers.Commands;

    public class CommandTests : TestBase
    {
        [Fact]
        public void Can_Handle_Insert_Customer()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer(name: "Innovation", userName: "somecrazynamethatdoesnotexistyet");

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
            var insertCustomerCommand = new InsertCustomer(name: "Innovation", userName: "somecrazynamethatdoesnotexistyet");

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
            var insertVendorCommand = new InsertVendor(name: "Innovation", userName: "SomeUserNameThatRocks");

            // Act
            var dispatcher = this.GetDispatcher();
            var insertVendorCommandResult = await dispatcher.Command(command: insertVendorCommand, suppressExceptions: false);

            // Assert
            Assert.False(condition: insertVendorCommandResult.Success);
            Assert.IsType<CommandResult>(@object: insertVendorCommandResult);
            Assert.Equal(expected: "Test Error Message", actual: ((CommandResult)insertVendorCommandResult).Errors.First());
        }
    }
}
