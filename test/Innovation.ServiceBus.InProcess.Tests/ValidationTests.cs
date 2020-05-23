namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;

    using Xunit;
    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Customers.Criteria;
    using ApiSample.Customers.Commands;
    using ApiSample.Suppliers.Criteria;
    using ApiSample.Suppliers.Commands;

    public class ValidationTests : TestBase
    {
        [Fact]
        public async Task Can_Handle_Required_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(name: null, userName: null);
            var insertCustomerCommand = new InsertCustomer(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: 2, actual: commandResult.Errors.Length);
            Assert.Equal(expected: "The Name field is required.", actual: commandResult.Errors[0]);
            Assert.Equal(expected: "The UserName field is required.", actual: commandResult.Errors[1]);
        }

        [Fact]
        public async Task Can_Handle_Min_Length_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(name: "aa", userName: "aa");
            var insertCustomerCommand = new InsertCustomer(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: 2, actual: commandResult.Errors.Length);
            Assert.Equal(expected: "Name needs to be between 3 and 30 characters", actual: commandResult.Errors[0]);
            Assert.Equal(expected: "UserName needs to be between 10 and 35 characters", actual: commandResult.Errors[1]);
        }

        [Fact]
        public async Task Can_Handle_Max_Length_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "0123456789012345678901234567890",
                userName: "0123456789012345678901234567890123456");
            var insertCustomerCommand = new InsertCustomer(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: 2, actual: commandResult.Errors.Length);
            Assert.Equal(expected: "Name needs to be between 3 and 30 characters", actual: commandResult.Errors[0]);
            Assert.Equal(expected: "UserName needs to be between 10 and 35 characters", actual: commandResult.Errors[1]);
        }

        [Fact]
        public async Task Can_Handle_Regex_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "0123#",
                userName: "0123456789@");
            var insertCustomerCommand = new InsertCustomer(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: 2, actual: commandResult.Errors.Length);
            Assert.Equal(expected: "Name only allows alphanumeric characters", actual: commandResult.Errors[0]);
            Assert.Equal(expected: "UserName only allows alphanumeric characters", actual: commandResult.Errors[1]);
        }

        [Fact]
        public async Task Can_Handle_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "I",
                userName: "somecrazynamethatdoesnotexistyet");
            var insertCustomerCommand = new InsertCustomer(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: "Name needs to be between 3 and 30 characters", actual: commandResult.Errors[0]);
        }

        [Fact]
        public async Task Can_Handle_IValidatableObject_On_Command_Property()
        {
            // Arrange
            var supplierCriteria = new SupplierCriteria(name: "Louis");
            var insertCustomerCommand = new InsertSupplier(supplierCriteria: supplierCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(condition: commandResult.Success);
            Assert.Equal(expected: "Name Cannot Be Louis", actual: commandResult.Errors[0]);
        }
    }
}
