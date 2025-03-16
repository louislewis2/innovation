namespace Innovation.Sample.BaseModule.Handlers.Customers.Queries
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using Innovation.Api.Querying;

    using Innovation.Sample.Api.Paging;
    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Data.Anemics.Customers;
    using Innovation.Sample.Api.Customers.ViewModels;

    public class CustomerLiteLoader :
        IQueryHandler<GetCustomerQuery, CustomerLite>,
        IQueryHandler<QueryPage, GenericResultsList<CustomerLite>>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly PrimaryContext primaryContext;

        #endregion Fields

        #region Constructor

        public CustomerLiteLoader(ILogger<CustomerLiteLoader> logger, PrimaryContext primaryContext)
        {
            this.logger = logger;
            this.primaryContext = primaryContext;
        }

        #endregion Constructor

        #region Methods

        public async Task<CustomerLite> Handle(GetCustomerQuery query)
        {
            return await Load(query: query);
        }

        public async Task<GenericResultsList<CustomerLite>> Handle(QueryPage query)
        {
            return await Load(query: query);
        }

        #endregion Methods

        #region Private Methods

        private async Task<CustomerLite> Load(GetCustomerQuery query)
        {
            var customer = await primaryContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.CustomerId);

            return customer.ToCustomerLite();
        }

        private async Task<GenericResultsList<CustomerLite>> Load(QueryPage query)
        {
            var serverCount = 0;
            var customersQueryable = primaryContext.Customers
                .AsNoTracking()
                .AsQueryable();

            // Opt in because it results in a extra database hit
            if (query.IncludeServerCount)
            {
                serverCount = await customersQueryable.CountAsync();
            }

            var itemsPaged = await customersQueryable.Page(queryPage: query).ToArrayAsync();

            return new GenericResultsList<CustomerLite>(itemsPaged.ToCustomerLite(), new QueryPagingInfo(query.Page, query.PageSize, serverCount));
        }

        #endregion Private Methods
    }
}
