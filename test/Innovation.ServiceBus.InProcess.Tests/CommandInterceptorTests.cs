namespace Innovation.ServiceBus.InProcess.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    using ApiSample.Customers.Criteria;
    using ApiSample.Customers.Commands;

    public class CommandInterceptorTests : TestBase
    {
        [Fact]
        public async Task Can_Run_Intercept_Expecting_False()
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
            Assert.NotNull(@object: insertCustomerCommand.Criteria.ExistsOnGithub);
            Assert.False(condition: insertCustomerCommand.Criteria.ExistsOnGithub.Value);
        }

        [Fact]
        public async Task Can_Run_Intercept_Expecting_True()
        {
            // Arrange
            var insertCustomerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "louislewis2");
            var insertCustomerCommand = new InsertCustomer(customerCriteria: insertCustomerCriteria);

            // Act
            var dispatcher = this.GetDispatcher();
            var insertCustomerCommandResult = await dispatcher.Command(command: insertCustomerCommand, suppressExceptions: false);

            // Assert
            Assert.NotNull(@object: insertCustomerCommand.Criteria.ExistsOnGithub);
            Assert.True(condition: insertCustomerCommand.Criteria.ExistsOnGithub.Value);
        }
    }
}
