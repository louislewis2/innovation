namespace Innovation.ServiceBus.InProcess.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;
    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Shared.Contexts;
    using ApiSample.Customers.Criteria;
    using ApiSample.Customers.Commands;

    public class DispatcherContextTests : TestBase
    {
        #region Methods

        [Fact]
        public async Task Can_Fail_Handle_Context_Not_Set_In_Command_Handler()
        {
            // Arrange
            var customerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "somecrazynamethatdoesnotexistyet");
            Guid customerId = Guid.NewGuid();
            var updateCustomerCommand = new UpdateCustomer(customerCriteria: customerCriteria, customerId: customerId);

            // Act
            var dispatcher = this.GetDispatcher();
            var updateCustomerCommandResult = (await dispatcher.Command(command: updateCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.False(condition: updateCustomerCommandResult.Success);
            Assert.Single(updateCustomerCommandResult.Errors);
            Assert.Equal(expected: "Dispatcher Context Not Set", actual: updateCustomerCommandResult.Errors[ 0 ]);
        }

        [Fact]
        public async Task Can_Get_Context_In_Command_Handler()
        {
            // Arrange
            var customerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "somecrazynamethatdoesnotexistyet");
            Guid customerId = Guid.NewGuid();
            var updateCustomerCommand = new UpdateCustomer(customerCriteria: customerCriteria, customerId: customerId);
            var sharedDispatcherContext = new SharedDispatcherContext();

            // Act
            var dispatcher = this.GetDispatcher();
            dispatcher.SetContext(dispatcherContext: sharedDispatcherContext);
            var updateCustomerCommandResult = (await dispatcher.Command(command: updateCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.True(condition: updateCustomerCommandResult.Success);
        }

        #endregion Methods
    }
}
