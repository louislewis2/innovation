namespace Innovation.Integration.AspNetCore
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Extensions.DependencyInjection;

    using Innovation.Api.Querying;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;

    public class InnovationBaseController : Controller
    {
        #region Properties

        public IDispatcher Dispatcher => this.GetDispatcher();

        #endregion Properties

        #region Methods

        protected async Task<TQueryResult> Query<TQuery, TQueryResult>(TQuery query)
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            return await this.Dispatcher.Query<TQuery, TQueryResult>(query);
        }

        protected async Task<ICommandResult> Command<TCommand>(TCommand command, bool suppressExceptions = true) where TCommand : ICommand
        {
            return await this.Dispatcher.Command(command: command, suppressExceptions: suppressExceptions);
        }

        #endregion Methods

        #region Private Methods

        private IDispatcher GetDispatcher()
        {
            var dispatcher = this.HttpContext.RequestServices.GetRequiredService<IDispatcher>();
            var correlationIdOptions = this.HttpContext.RequestServices.GetRequiredService<IOptions<CorrelationIdOptions>>().Value;

            if (this.HttpContext.Request.Headers.TryGetValue(correlationIdOptions.Header, out StringValues correlationId))
            {
                if (!string.IsNullOrWhiteSpace(value: correlationId))
                {
                    dispatcher.SetCorrelationId(correlationId);
                }
            }

            return dispatcher;
        }

        #endregion Private Methods
    }
}
