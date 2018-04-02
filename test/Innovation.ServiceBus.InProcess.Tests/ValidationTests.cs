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
        public async Task Can_Handle_Required_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer(name: null, userName: null);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(commandResult.Success);
            Assert.Equal(expected: 2, actual: commandResult.Errors.Length);
            Assert.Equal("The Name field is required.", commandResult.Errors[0]);
            Assert.Equal("The UserName field is required.", commandResult.Errors[1]);
        }

        [Fact]
        public async Task Can_Handle_Min_Length_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer(name: "aa", userName: "aa");

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(commandResult.Success);
            Assert.Equal(expected: 2, actual: commandResult.Errors.Length);
            Assert.Equal("Name needs to be between 3 and 30 characters", commandResult.Errors[0]);
            Assert.Equal("UserName needs to be between 10 and 35 characters", commandResult.Errors[1]);
        }

        [Fact]
        public async Task Can_Handle_Max_Length_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer(
                name: "0123456789012345678901234567890",
                userName: "0123456789012345678901234567890123456");

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(commandResult.Success);
            Assert.Equal(expected: 2, actual: commandResult.Errors.Length);
            Assert.Equal("Name needs to be between 3 and 30 characters", commandResult.Errors[0]);
            Assert.Equal("UserName needs to be between 10 and 35 characters", commandResult.Errors[1]);
        }

        [Fact]
        public async Task Can_Handle_Regex_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer(
                name: "0123#",
                userName: "0123456789@");

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(commandResult.Success);
            Assert.Equal(expected: 2, actual: commandResult.Errors.Length);
            Assert.Equal("Name only allows alphanumeric characters", commandResult.Errors[0]);
            Assert.Equal("UserName only allows alphanumeric characters", commandResult.Errors[1]);
        }

        [Fact]
        public async Task Can_Handle_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCommand = new InsertCustomer(name: "I", userName: "somecrazynamethatdoesnotexistyet");

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(commandResult.Success);
            Assert.Equal("Name needs to be between 3 and 30 characters", commandResult.Errors[0]);
        }
    }
}
