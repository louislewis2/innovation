namespace Innovation.ServiceBus.InProcess.Tests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Innovation.ServiceBus.InProcess.Validators;

    using ApiSample.Customers.Commands;
    using ApiSample.Customers.Criteria;
    using ApiSample.Suppliers.Commands;
    using ApiSample.Suppliers.Criteria;

    [TestClass]
    public class DataAnnotationsValidatorTests : TestBase
    {
        [TestMethod]
        public async Task Can_Handle_Required_Validation_Attributes()
        {
            // Arrange
            var serviceProvider = this.GetService<IServiceProvider>();
            var dataAnnotationsValidator = new DataAnnotationsValidator(serviceProvider);
            var insertCustomerCriteria = new CustomerCriteria(name: null, userName: null);
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var validatorResult = await dataAnnotationsValidator.TryValidateObjectRecursive(insertCustomerCommand);

            // Assert
            Assert.IsFalse(condition: validatorResult.isValid);
            Assert.IsNotNull(validatorResult.Errors);
            Assert.AreEqual(expected: 2, actual: validatorResult.Errors.Count);
            Assert.AreEqual(expected: "The Name field is required.", actual: validatorResult.Errors["Criteria.Name"][0]);
            Assert.AreEqual(expected: "The UserName field is required.", actual: validatorResult.Errors["Criteria.UserName"][0]);
        }

        [TestMethod]
        public async Task Can_Handle_Min_Length_Validation_Attributes()
        {
            // Arrange
            var serviceProvider = this.GetService<IServiceProvider>();
            var dataAnnotationsValidator = new DataAnnotationsValidator(serviceProvider);
            var insertCustomerCriteria = new CustomerCriteria(name: "aa", userName: "aa");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var validatorResult = await dataAnnotationsValidator.TryValidateObjectRecursive(insertCustomerCommand);

            // Assert
            Assert.IsFalse(condition: validatorResult.isValid);
            Assert.IsNotNull(validatorResult.Errors);
            Assert.AreEqual(expected: 2, actual: validatorResult.Errors.Count);
            Assert.AreEqual(expected: "Name needs to be between 3 and 30 characters", actual: validatorResult.Errors["Criteria.Name"][0]);
            Assert.AreEqual(expected: "UserName needs to be between 10 and 35 characters", actual: validatorResult.Errors["Criteria.UserName"][0]);
        }

        [TestMethod]
        public async Task Can_Handle_Max_Length_Validation_Attributes()
        {
            // Arrange
            var serviceProvider = this.GetService<IServiceProvider>();
            var dataAnnotationsValidator = new DataAnnotationsValidator(serviceProvider);
            var insertCustomerCriteria = new CustomerCriteria(
                name: "0123456789012345678901234567890",
                userName: "0123456789012345678901234567890123456");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var validatorResult = await dataAnnotationsValidator.TryValidateObjectRecursive(insertCustomerCommand);

            // Assert
            Assert.IsFalse(condition: validatorResult.isValid);
            Assert.IsNotNull(validatorResult.Errors);
            Assert.AreEqual(expected: 2, actual: validatorResult.Errors.Count);
            Assert.AreEqual(expected: "Name needs to be between 3 and 30 characters", actual: validatorResult.Errors["Criteria.Name"][0]);
            Assert.AreEqual(expected: "UserName needs to be between 10 and 35 characters", actual: validatorResult.Errors["Criteria.UserName"][0]);
        }

        [TestMethod]
        public async Task Can_Handle_Regex_Validation_Attributes()
        {
            // Arrange
            var serviceProvider = this.GetService<IServiceProvider>();
            var dataAnnotationsValidator = new DataAnnotationsValidator(serviceProvider);
            var insertCustomerCriteria = new CustomerCriteria(
                name: "0123#",
                userName: "0123456789@");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var validatorResult = await dataAnnotationsValidator.TryValidateObjectRecursive(insertCustomerCommand);

            // Assert
            Assert.IsFalse(condition: validatorResult.isValid);
            Assert.IsNotNull(validatorResult.Errors);
            Assert.AreEqual(expected: 2, actual: validatorResult.Errors.Count);
            Assert.AreEqual(expected: "Name only allows alphanumeric characters", actual: validatorResult.Errors["Criteria.Name"][0]);
            Assert.AreEqual(expected: "UserName only allows alphanumeric characters", actual: validatorResult.Errors["Criteria.UserName"][0]);
        }

        [TestMethod]
        public async Task Can_Handle_Validation_Attributes()
        {
            // Arrange
            var serviceProvider = this.GetService<IServiceProvider>();
            var dataAnnotationsValidator = new DataAnnotationsValidator(serviceProvider);
            var insertCustomerCriteria = new CustomerCriteria(
                name: "I",
                userName: "somecrazynamethatdoesnotexistyet");
            var insertCustomerCommand = new InsertCustomerCommand(customerCriteria: insertCustomerCriteria);

            // Act
            var validatorResult = await dataAnnotationsValidator.TryValidateObjectRecursive(insertCustomerCommand);

            // Assert
            Assert.IsFalse(condition: validatorResult.isValid);
            Assert.IsNotNull(validatorResult.Errors);
            Assert.AreEqual(expected: "Name needs to be between 3 and 30 characters", actual: validatorResult.Errors["Criteria.Name"][0]);
        }

        [TestMethod]
        public async Task Can_Handle_IValidatableObject_On_Command_Property()
        {
            // Arrange
            var serviceProvider = this.GetService<IServiceProvider>();
            var dataAnnotationsValidator = new DataAnnotationsValidator(serviceProvider);
            var supplierCriteria = new SupplierCriteria(name: "Louis");
            var insertCustomerCommand = new InsertSupplierCommand(supplierCriteria: supplierCriteria);

            // Act
            var validatorResult = await dataAnnotationsValidator.TryValidateObjectRecursive(insertCustomerCommand);

            // Assert
            Assert.IsFalse(condition: validatorResult.isValid);
            Assert.IsNotNull(validatorResult.Errors);
            Assert.AreEqual(expected: "Name Cannot Be Louis", actual: validatorResult.Errors["Criteria.Name"][0]);
        }
    }
}
