namespace Innovation.Sample.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Innovation.Integration.AspNetCore;

    using Innovation.Sample.Api.Paging;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Api.Customers.Commands;
    using Innovation.Sample.Api.Customers.Criteria;
    using Innovation.Sample.Api.Customers.ViewModels;

    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomerController : InnovationBaseController
    {
        #region Methods

        public async Task<IActionResult> Index(QueryPage queryPage)
        {
            return this.View(await this.Query<QueryPage, GenericResultsList<CustomerLite>>(query: queryPage));
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            var createCustomerCriteria = CreateCustomerCriteria.Default();

            return View(createCustomerCriteria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerCriteria createCustomerCriteria)
        {
            if (!ModelState.IsValid)
            {
                return View(createCustomerCriteria);
            }

            var createCustomerCommand = new CreateCustomerCommand(createCustomerCriteria: createCustomerCriteria);
            var createCustomerCommandResult = await this.Dispatcher.Command(command: createCustomerCommand);

            if (createCustomerCommandResult.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // TODO : Return errors from the command result
                return View(createCustomerCriteria);
            }
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(Guid customerId)
        {
            if (customerId == default)
            {
                return NotFound();
            }

            var customer = await this.GetCustomerDetail(customerId: customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(Guid customerId)
        {
            if (customerId == default)
            {
                return NotFound();
            }

            var customer = await this.GetCustomerDetail(customerId: customerId);

            if (customer == null)
            {
                return NotFound();
            }

            var updateCustomerCriteria = new UpdateCustomerCriteria(
                firstName: customer.FirstName,
                lastName: customer.LastName,
                email: customer.Email,
                phoneNumber: customer.PhoneNumber);

            ViewBag.customerId = customerId;
            return View(updateCustomerCriteria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid customerId, UpdateCustomerCriteria updateCustomerCriteria)
        {
            if (customerId == default)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(updateCustomerCriteria);
            }

            var updateCustomerCommand = new UpdateCustomerCommand(customerId: customerId, updateCustomerCriteria: updateCustomerCriteria);
            var updateCustomerCommandResult = await this.Dispatcher.Command(command: updateCustomerCommand);

            if (updateCustomerCommandResult.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // TODO : Return errors from the command result
                return View(updateCustomerCommand);
            }
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(Guid customerId)
        {
            if (customerId == default)
            {
                return NotFound();
            }

            var customer = await this.GetCustomerLite(customerId: customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid customerId)
        {
            if (customerId == default)
            {
                return NotFound();
            }

            var deleteCustomerCommand = new DeleteCustomerCommand(customerId: customerId);
            var deleteCustomerCommandResult = await this.Command(command: deleteCustomerCommand);

            return RedirectToAction("Index");
        }

        #endregion Methods

        #region Private Methods

        private async Task<CustomerLite> GetCustomerLite(Guid customerId)
        {
            var getCustomerQuery = new GetCustomerQuery(customerId: customerId);
            return await this.Query<GetCustomerQuery, CustomerLite>(getCustomerQuery);
        }

        private async Task<CustomerDetail> GetCustomerDetail(Guid customerId)
        {
            var getCustomerQuery = new GetCustomerQuery(customerId: customerId);
            return await this.Query<GetCustomerQuery, CustomerDetail>(getCustomerQuery);
        }

        #endregion Private Methods
    }
}
