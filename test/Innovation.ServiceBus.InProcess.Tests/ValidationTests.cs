namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Customers.Criteria;
    using ApiSample.Customers.Commands;
    using ApiSample.Suppliers.Criteria;
    using ApiSample.Suppliers.Commands;

    [TestClass]
    public class ValidationTests : TestBase
    {
        [TestMethod]
        public async Task Can_Handle_Required_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(name: null, userName: null);
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.IsFalse(condition: commandResult.Success);
            Assert.AreEqual(expected: 2, actual: commandResult.Errors.Length());
            Assert.AreEqual(expected: "The Name field is required.", actual: commandResult[0].Reasons[0]);
            Assert.AreEqual(expected: "The UserName field is required.", actual: commandResult[1].Reasons[0]);
        }

        [TestMethod]
        public async Task Can_Handle_Min_Length_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(name: "aa", userName: "aa");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.IsFalse(condition: commandResult.Success);
            Assert.AreEqual(expected: 2, actual: commandResult.Errors.Length());
            Assert.AreEqual(expected: "Name needs to be between 3 and 30 characters", actual: commandResult[0].Reasons[0]);
            Assert.AreEqual(expected: "UserName needs to be between 10 and 35 characters", actual: commandResult[1].Reasons[0]);
        }

        [TestMethod]
        public async Task Can_Handle_Max_Length_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "0123456789012345678901234567890",
                userName: "0123456789012345678901234567890123456");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.IsFalse(condition: commandResult.Success);
            Assert.AreEqual(expected: 2, actual: commandResult.Errors.Length());
            Assert.AreEqual(expected: "Name needs to be between 3 and 30 characters", actual: commandResult[0].Reasons[0]);
            Assert.AreEqual(expected: "UserName needs to be between 10 and 35 characters", actual: commandResult[1].Reasons[0]);
        }

        [TestMethod]
        public async Task Can_Handle_Regex_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "0123#",
                userName: "0123456789@");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.IsFalse(condition: commandResult.Success);
            Assert.AreEqual(expected: 2, actual: commandResult.Errors.Length());
            Assert.AreEqual(expected: "Name only allows alphanumeric characters", actual: commandResult[0].Reasons[0]);
            Assert.AreEqual(expected: "UserName only allows alphanumeric characters", actual: commandResult[1].Reasons[0]);
        }

        [TestMethod]
        public async Task Can_Handle_Validation_Attributes()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "I",
                userName: "somecrazynamethatdoesnotexistyet");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.IsFalse(condition: commandResult.Success);
            Assert.AreEqual(expected: "Name needs to be between 3 and 30 characters", actual: commandResult[0].Reasons[0]);
        }

        [TestMethod]
        public async Task Can_Handle_IValidatableObject_On_Command_Property()
        {
            // Arrange
            var supplierCriteria = new SupplierCriteria(name: "Louis");
            var insertCustomerCommand = new InsertSupplierCommand(supplierCriteria: supplierCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var commandResult = (await dispatcher.Command(command: insertCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.IsFalse(condition: commandResult.Success);
            Assert.AreEqual(expected: "Name Cannot Be Louis", actual: commandResult[0].Reasons[0]);
        }
    }
}
