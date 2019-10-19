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

    public class CustomerController : InnovationBaseController
    {
        #region Methods

        public async Task<IActionResult> Index(QueryPage queryPage)
        {
            return this.View(await this.Query<QueryPage, GenericResultsList<CustomerLite>>(queryPage));
        }

        // GET: People/Create
        public IActionResult Create()
        {
            var command = new CreateCustomer();

            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomer createCustomer)
        {
            if (ModelState.IsValid)
            {
                var commandResult = await this.Command(createCustomer);

                if (commandResult.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // TODO : Return errors from the command result
                    return View(createCustomer);
                }
            }
            return View(createCustomer);
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await this.GetCustomerDetail(id.Value);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await this.GetCustomerDetail(id.Value);

            if (customer == null)
            {
                return NotFound();
            }

            var updateCustomer = new UpdateCustomer()
            {
                Id = id.Value,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone
            };

            return View(updateCustomer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateCustomer updateCustomerCommand)
        {
            if (id != updateCustomerCommand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var commandResult = await this.Command(updateCustomerCommand);

                if (commandResult.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // TODO : Return errors from the command result
                    return View(updateCustomerCommand);
                }
            }

            return View(updateCustomerCommand);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await this.GetCustomerLite(id.Value);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var command = new DeleteCustomer(id: id);
            var commandResult = await this.Command(command);

            return RedirectToAction("Index");
        }

        #endregion Methods

        #region Private Methods

        private async Task<CustomerLite> GetCustomerLite(Guid id)
        {
            var getCustomer = new GetCustomer(id: id);
            return await this.Query<GetCustomer, CustomerLite>(getCustomer);
        }

        private async Task<CustomerDetail> GetCustomerDetail(Guid id)
        {
            var getCustomer = new GetCustomer(id: id);
            return await this.Query<GetCustomer, CustomerDetail>(getCustomer);
        }

        #endregion Private Methods
    }
}
