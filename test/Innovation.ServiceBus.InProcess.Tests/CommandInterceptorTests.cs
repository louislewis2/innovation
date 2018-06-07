namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    using Innovation.ApiSample.Customers.Commands;

    public class CommandInterceptorTests : TestBase
    {
        [Fact]
        public async Task Can_Run_Intercept_Expecting_False()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer(name: "Innovation", userName: "somecrazynamethatdoesnotexistyet");

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(command: insertCustomerCommand, suppressExceptions: false);

            // Assert
            Assert.NotNull(@object: insertCustomerCommand.ExistsOnGithub);
            Assert.False(condition: insertCustomerCommand.ExistsOnGithub.Value);
        }

        [Fact]
        public async Task Can_Run_Intercept_Expecting_True()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer(name: "Innovation", userName: "louislewis2");

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(command: insertCustomerCommand, suppressExceptions: false);

            // Assert
            Assert.NotNull(@object: insertCustomerCommand.ExistsOnGithub);
            Assert.True(condition: insertCustomerCommand.ExistsOnGithub.Value);
        }
    }
}
