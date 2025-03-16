namespace Innovation.Integration.AspNetCore
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.DependencyInjection;

    using Innovation.Api.Querying;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;
    using Innovation.Api.CommandHelpers;

    public class InnovationBaseController : Controller
    {
        #region Fields

        private static Dictionary<string, string[]> resourceNotFoundErrorDictionary = new Dictionary<string, string[]> 
            { 
                { "Resource", new[] {"Resource not found with provided resourceId"} },
            };

        private static CommandResult resourceNotFoundCommandResult = new CommandResult(success: false, recordId: null, errors: resourceNotFoundErrorDictionary);

        #endregion Fields

        #region Properties

        public IDispatcher Dispatcher => this.GetDispatcher();
        public IHostEnvironment HostingEnvironment => this.GetGostingEnvironment();

        #endregion Properties

        #region Methods

        protected async Task<TQueryResult> Query<TQuery, TQueryResult>([DisallowNull] TQuery query)
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            return await this.Dispatcher.Query<TQuery, TQueryResult>(query);
        }

        protected async Task<ObjectResult> Query<TQuery, TQueryResult>([DisallowNull] TQuery query, string identifer)
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            return await this.QueryHandleResourceNotFound<TQuery, string, TQueryResult>(query, identifer);
        }

        protected async Task<ObjectResult> Query<TQuery, TQueryResult>([DisallowNull] TQuery query, Guid identifer)
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            return await this.QueryHandleResourceNotFound<TQuery, Guid, TQueryResult>(query, identifer);
        }

        protected async Task<ObjectResult> Query<TQuery, TQueryResult>([DisallowNull] TQuery query, int identifer)
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            return await this.QueryHandleResourceNotFound<TQuery, int, TQueryResult>(query, identifer);
        }

        protected async Task<ObjectResult> Query<TQuery, TQueryResult>([DisallowNull] TQuery query, long identifer)
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            return await this.QueryHandleResourceNotFound<TQuery, long, TQueryResult>(query, identifer);
        }

        protected async ValueTask<ObjectResult> Command<TCommand>([DisallowNull] TCommand command, bool suppressExceptions = true) where TCommand : ICommand
        {
            var commandResult = await this.Dispatcher.Command(command: command, suppressExceptions: suppressExceptions);

            if (commandResult.Success)
            {
                return this.Ok(value: commandResult);
            }

            return this.BadRequest(error: commandResult);
        }

        protected ObjectResult CreateErrorFromMessage([DisallowNull] string memberName, [DisallowNull] string message)
        {
            var commandResult = new CommandResult(memberName: memberName, errorMessage: message);

            return this.BadRequest(commandResult);
        }

        protected ObjectResult CreateErrorFromMessage([DisallowNull] string message)
        {
            var commandResult = new CommandResult(errorMessage: message);

            return this.BadRequest(commandResult);
        }

        protected ObjectResult HandleModelStateErrors()
        {
            var commandResult = this.ModelState.Handle();

            return this.BadRequest(commandResult);
        }

        protected ObjectResult GenerateReturnResult(ICommandResult commandResult)
        {
            if (!commandResult.Success)
            {
                return this.BadRequest(error: commandResult);
            }

            return this.Ok(value: commandResult);
        }

        protected ObjectResult GenerateReturnResult([DisallowNull] Exception ex, bool logAsWarning = false)
        {
            var logger = this.GetLogger();

            if (logAsWarning)
            {
                logger.LogWarning(exception: ex, message: ex.GetInnerMostMessage());
            }
            else
            {
                logger.LogError(exception: ex, message: ex.GetInnerMostMessage());
            }

            var finalMessage = this.HostingEnvironment.IsProduction()
                ? "An Exception Occurred"
                : ex.GetInnerMostMessage();

            var commandResult = new CommandResult(errorMessage: finalMessage);

            return this.BadRequest(error: commandResult);
        }

        protected ObjectResult GenerateReturnResult(IQueryResult queryresult, Guid resourceId)
        {
            if (queryresult == null)
            {
                return this.NotFound(value: $"Resource with identity `{resourceId}` could not be found");
            }

            return this.Ok(value: queryresult);
        }

        protected IHeaderDictionary GetHeaders()
        {
            return this.HttpContext.Request.Headers;
        }

        protected bool DoesHeaderExist([DisallowNull] string key)
        {
            return this.HttpContext.Request.Headers.ContainsKey(key);
        }

        protected string GetHeader([DisallowNull] string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            if (this.HttpContext == null || this.HttpContext.Request == null || this.HttpContext.Request.Headers == null)
            {
                return string.Empty;
            }

            if (this.HttpContext.Response.Headers.ContainsKey(key: key))
            {
                return this.HttpContext.Response.Headers[key];
            }

            return string.Empty;
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

        private ILogger GetLogger()
        {
            return this.HttpContext.RequestServices.GetRequiredService<ILogger<InnovationBaseController>>();
        }

        private IHostEnvironment GetGostingEnvironment()
        {
            return this.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();
        }

        private async Task<ObjectResult> QueryHandleResourceNotFound<TQuery, TResourceId, TQueryResult>([DisallowNull] TQuery query, TResourceId resourceId)
            where TQuery : IQuery
            where TQueryResult : IQueryResult
        {
            var queryResult = await this.Dispatcher.Query<TQuery, TQueryResult>(query);

            if (queryResult == null)
            {
                return this.BadRequest(resourceNotFoundCommandResult);
            }

            return this.Ok(queryResult);
        }

        #endregion Private Methods
    }
}
