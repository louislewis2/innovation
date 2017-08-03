namespace Innovation.Sample.Data.Handlers.Customers.Queries
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;

    using Innovation.Api.Querying;
    using Innovation.Sample.Api.Paging;
    using Innovation.Sample.Data.Anemics;
    using Innovation.Sample.Data.Contexts;
    using Innovation.Sample.Api.Customers.Queries;
    using Innovation.Sample.Api.Customers.ViewModels;

    public class CustomerLiteLoader :
        IQueryHandler<GetCustomer, CustomerLite>,
        IQueryHandler<QueryPage, GenericResultsList<CustomerLite>>
    {
        #region Fields

        private readonly ILogger logger;
        private readonly ExampleContext context;

        #endregion Fields

        #region Constructor

        public CustomerLiteLoader(ILogger<CustomerLiteLoader> logger, ExampleContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        #endregion Constructor

        #region Methods

        public async Task<CustomerLite> Handle(GetCustomer query)
        {
            return await this.Load(query: query);
        }

        public async Task<GenericResultsList<CustomerLite>> Handle(QueryPage query)
        {
            return await this.Load(query: query);
        }

        #endregion Methods

        #region Private Methods

        private async Task<CustomerLite> Load(GetCustomer query)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(x => x.Id == query.Id);

            return customer == null ? null : this.Convert(customer);
        }

        private async Task<GenericResultsList<CustomerLite>> Load(QueryPage query)
        {
            var serverCount = 0;
            var customersQueryable = this.context.Customers.AsQueryable();

            // Opt in because it results in a extra database hit
            if (query.IncludeServerCount)
            {
                serverCount = await customersQueryable.CountAsync();
            }

            var itemsPaged = await customersQueryable.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToArrayAsync();

            return new GenericResultsList<CustomerLite>(itemsPaged.Select(this.Convert).ToArray(), new QueryPagingInfo(query.Page, query.PageSize, serverCount));
        }

        private CustomerLite Convert(AnemicCustomer input)
        {
            return new CustomerLite(
                id: input.Id,
                fullName: $"{input.FirstName} - {input.LastName}"
                );
        }

        #endregion Private Methods
    }
}
