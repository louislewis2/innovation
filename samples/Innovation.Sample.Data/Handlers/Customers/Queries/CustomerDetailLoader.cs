namespace Innovation.Sample.Data.Handlers.Customers.Queries
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using Innovation.Api.Querying;

    using Innovation.Sample.Data.Anemics;
    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Api.Customers.ViewModels;

    public class CustomerDetailLoader : IQueryHandler<GetCustomer, CustomerDetail>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly ExampleContext context;

        #endregion Fields

        #region Constructor

        public CustomerDetailLoader(ILogger<CustomerDetailLoader> logger, ExampleContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        #endregion Constructor

        #region Methods

        public async Task<CustomerDetail> Handle(GetCustomer query)
        {
            return await this.Load(query: query);
        }

        #endregion Methods

        #region Private Methods

        private async Task<CustomerDetail> Load(GetCustomer query)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == query.Id);

            return customer == null ? null : this.Convert(customer);
        }

        private CustomerDetail Convert(AnemicCustomer input)
        {
            return new CustomerDetail
            {
                Id = input.Id,
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                Phone = input.PhoneNumber,
            };
        }

        #endregion Private Methods
    }
}
