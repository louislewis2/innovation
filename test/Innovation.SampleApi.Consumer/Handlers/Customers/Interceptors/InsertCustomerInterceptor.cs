namespace Innovation.SampleApi.Consumer.Handlers.Customers.Interceptors
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Octokit;
    using Innovation.Api.Interceptors;

    using ApiSample.Customers.Commands;

    public class InsertCustomerInterceptor : ICommandInterceptor<InsertCustomerCommand>
    {
        #region Fields

        private readonly ILogger logger;

        #endregion Fields

        #region Constructor

        public InsertCustomerInterceptor(ILogger<InsertCustomerInterceptor> logger)
        {
            this.logger = logger;
        }

        #endregion Constructor

        #region Methods

        public async Task Intercept(InsertCustomerCommand command)
        {
            await this.HandleIntercept(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task HandleIntercept(InsertCustomerCommand command)
        {
            try
            {
                var github = new GitHubClient(productInformation: new ProductHeaderValue("Innovation"));
                var user = await github.User.Get(login: command.Criteria.UserName);

                var exists = user != null;

                command.Criteria.SetGithubStatus(existsOnGithub: exists);
            }
            catch (Exception ex)
            {
                this.logger.LogError(exception: ex, ex.GetInnerMostMessage());

                command.Criteria.SetGithubStatus(existsOnGithub: false);
            }
        }

        #endregion Private Methods
    }
}
