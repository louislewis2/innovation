namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;
    using Innovation.ApiSample.Customers.Commands;

    public class ValidationTests : TestBase
    {
        [Fact]
        public async Task Can_Handle_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer("I", "somecrazynamethatdoesnotexistyet");

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(commandResult.Success);
            Assert.Equal("Name needs to be between 3 and 30 characters", commandResult.Errors[0]);
        }
    }
}
