namespace Innovation.Sample.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Innovation.Integration.AspNetCore;

    using Innovation.Sample.Api.Paging;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Api.Customers.Commands;
    using Innovation.Sample.Api.Customers.ViewModels;

    [Route("api/[controller]")]
    public class CustomersController : InnovationBaseController
    {
        #region Methods

        [HttpGet]
        public async Task<IActionResult> Query(QueryPage queryPage)
        {
            return this.Ok(await this.Dispatcher.Query<QueryPage, GenericResultsList<CustomerLite>>(queryPage));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Single(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.BadRequest("A valid id is required");
            }

            var query = new GetCustomer(id: id);

            var customerViewModel = await this.Dispatcher.Query<GetCustomer, CustomerLite>(query);

            if (customerViewModel == null)
            {
                return this.NotFound($"Resource with identifier '{id}' not found");
            }

            return this.Ok(customerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCustomer command)
        {
            if (command == null)
            {
                return this.BadRequest("command is required");
            }

            return this.Ok(await this.Dispatcher.Command(command: command));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.BadRequest("A valid id is required");
            }

            var command = new DeleteCustomer(id: id);

            return this.Ok(await this.Dispatcher.Command(command: command));
        }

        #endregion Methods
    }
}
