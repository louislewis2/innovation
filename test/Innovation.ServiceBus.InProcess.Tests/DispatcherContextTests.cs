namespace Innovation.ServiceBus.InProcess.Tests
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    using ApiSample.Shared.Contexts;
    using ApiSample.Customers.Criteria;
    using ApiSample.Customers.Commands;

    [TestClass]
    public class DispatcherContextTests : TestBase
    {
        #region Methods

        [TestMethod]
        public async Task Can_Fail_Handle_Context_Not_Set_In_Command_Handler()
        {
            // Arrange
            var customerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "somecrazynamethatdoesnotexistyet");
            Guid customerId = Guid.NewGuid();
            var updateCustomerCommand = new UpdateCustomerCommand(customerCriteria: customerCriteria, customerId: customerId);

            // Act
            var dispatcher = this.GetDispatcher();
            var updateCustomerCommandResult = (await dispatcher.Command(command: updateCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.IsFalse(condition: updateCustomerCommandResult.Success);
            Assert.AreEqual(1, updateCustomerCommandResult.Errors.Length());
            Assert.AreEqual(expected: "Dispatcher Context Not Set", actual: updateCustomerCommandResult[0].Reasons[0]);
        }

        [TestMethod]
        public async Task Can_Get_Context_In_Command_Handler()
        {
            // Arrange
            var customerCriteria = new CustomerCriteria(
                name: "Innovation",
                userName: "somecrazynamethatdoesnotexistyet");
            Guid customerId = Guid.NewGuid();
            var updateCustomerCommand = new UpdateCustomerCommand(customerCriteria: customerCriteria, customerId: customerId);
            var sharedDispatcherContext = new SharedDispatcherContext();

            // Act
            var dispatcher = this.GetDispatcher();
            dispatcher.SetContext(dispatcherContext: sharedDispatcherContext);
            var updateCustomerCommandResult = (await dispatcher.Command(command: updateCustomerCommand)).As<CommandResult>();

            // Assert
            Assert.IsTrue(condition: updateCustomerCommandResult.Success);
        }

        #endregion Methods
    }
}
