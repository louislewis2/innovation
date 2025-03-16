namespace Innovation.Sample.BaseModule.Handlers.Customers.Queries
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using Innovation.Api.Querying;

    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Data.Anemics.Customers;
    using Innovation.Sample.Api.Customers.ViewModels;

    public class CustomerDetailLoader : IQueryHandler<GetCustomerQuery, CustomerDetail>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly PrimaryContext primaryContext;

        #endregion Fields

        #region Constructor

        public CustomerDetailLoader(ILogger<CustomerDetailLoader> logger, PrimaryContext primaryContext)
        {
            this.logger = logger;
            this.primaryContext = primaryContext;
        }

        #endregion Constructor

        #region Methods

        public async Task<CustomerDetail> Handle(GetCustomerQuery query)
        {
            return await Load(query: query);
        }

        #endregion Methods

        #region Private Methods

        private async Task<CustomerDetail> Load(GetCustomerQuery query)
        {
            var customer = await primaryContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.CustomerId);

            return customer.ToCustomerDetail();
        }

        #endregion Private Methods
    }
}
