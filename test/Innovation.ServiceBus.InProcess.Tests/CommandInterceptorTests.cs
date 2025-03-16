namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ApiSample.Customers.Criteria;
    using ApiSample.Customers.Commands;

    [TestClass]
    public class CommandInterceptorTests : TestBase
    {
        [TestMethod]
        public async Task Can_Run_Intercept_Expecting_False()
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
            Assert.IsNotNull(value: insertCustomerCommand.Criteria.ExistsOnGithub);
            Assert.IsFalse(condition: insertCustomerCommand.Criteria.ExistsOnGithub.Value);
        }

        [TestMethod]
        public async Task Can_Run_Intercept_Expecting_True()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "louislewis2");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(command: insertCustomerCommand, suppressExceptions: false);

            // Assert
            Assert.IsNotNull(value: insertCustomerCommand.Criteria.ExistsOnGithub);
            Assert.IsTrue(condition: insertCustomerCommand.Criteria.ExistsOnGithub.Value);
        }
    }
}
