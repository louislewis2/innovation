namespace Innovation.Sample.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Innovation.Api.CommandHelpers;
    using Innovation.Integration.AspNetCore;

    using Innovation.Sample.Api.Paging;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Api.Customers.Criteria;
    using Innovation.Sample.Api.Customers.Commands;
    using Innovation.Sample.Api.Customers.ViewModels;

    /// <summary>
    /// Exposes functionality relating to customers
    /// </summary>
    [Route("api/[controller]")]
    public class CustomersController : InnovationBaseController
    {
        #region Methods

        /// <summary>
        /// Provides server side paging functionality for retrieving customers
        /// </summary>
        /// <param name="queryPage">Provides settings for performing server side paging</param>
        /// <returns>Returns paged results, according to the provided criteria</returns>
        [HttpGet]
        [ProducesResponseType(type: typeof(GenericResultsList<CustomerLite>), statusCode: 200)]
        [ProducesResponseType(type: typeof(CommandResult), statusCode: 400)]
        public async Task<IActionResult> Query(QueryPage queryPage)
        {
            try
            {
                var genericResultsList = await this.Query<QueryPage, GenericResultsList<CustomerLite>>(query: queryPage);

                return this.Ok(value: genericResultsList);
            }
            catch(Exception ex)
            {
                return this.GenerateReturnResult(ex: ex);
            }
        }

        /// <summary>
        /// Loads a single customer, based on the unique customerId provided
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>Return a CustomerLite object if the record exists</returns>
        [ProducesResponseType(type: typeof(CustomerLite), statusCode: 200)]
        [ProducesResponseType(type: typeof(CommandResult), statusCode: 400)]
        [HttpGet("{customerId:Guid}")]
        public async Task<IActionResult> Single(Guid customerId)
        {
            try
            {
                var getCustomerQuery = new GetCustomerQuery(customerId: customerId);

                return await this.Query<GetCustomerQuery, CustomerLite>(query: getCustomerQuery, customerId);
            }
            catch (Exception ex)
            {
                return this.GenerateReturnResult(ex: ex);
            }
        }

        /// <summary>
        /// Inserts a new customer
        /// </summary>
        /// <param name="createCustomerCriteria"></param>
        /// <returns>An objecting indicating whethere the insert was suceful or not</returns>
        [ProducesResponseType(type: typeof(CommandResult), statusCode: 200)]
        [ProducesResponseType(type: typeof(CommandResult), statusCode: 400)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCriteria createCustomerCriteria)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.HandleModelStateErrors();
                }

                var createCustomerCommand = new CreateCustomerCommand(createCustomerCriteria: createCustomerCriteria);

                return await this.Command(command: createCustomerCommand);
            }
            catch (Exception ex)
            {
                return this.GenerateReturnResult(ex: ex);
            }
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="customerId">The customerId to be deleted</param>
        /// <returns>An objecting indicating whethere the delete was suceful or not</returns>
        [ProducesResponseType(type: typeof(CommandResult), statusCode: 200)]
        [ProducesResponseType(type: typeof(CommandResult), statusCode: 400)]
        [HttpDelete("{customerId:Guid}")]
        public async Task<IActionResult> Delete(Guid customerId)
        {
            try
            {
                if (customerId == default)
                {
                    return this.CreateErrorFromMessage("A valid id is required");
                }

                var deleteCustomerCommand = new DeleteCustomerCommand(customerId: customerId);

                return await this.Command(command: deleteCustomerCommand);
            }
            catch (Exception ex)
            {
                return this.GenerateReturnResult(ex: ex);
            }
        }

        #endregion Methods
    }
}
