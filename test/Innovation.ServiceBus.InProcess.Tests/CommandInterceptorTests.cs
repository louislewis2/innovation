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
            var insertCustomerCommand = new InsertCustomer("Innovation", "somecrazynamethatdoesnotexistyet");

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(insertCustomerCommand, false);

            // Assert
            Assert.NotNull(insertCustomerCommand.ExistsOnGithub);
            Assert.False(insertCustomerCommand.ExistsOnGithub.Value);
        }

        [Fact]
        public async Task Can_Run_Intercept_Expecting_True()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer("Innovation", "louislewis2");

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(insertCustomerCommand, false);

            // Assert
            Assert.NotNull(insertCustomerCommand.ExistsOnGithub);
            Assert.True(insertCustomerCommand.ExistsOnGithub.Value);
        }
    }
}
