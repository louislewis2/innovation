﻿namespace Innovation.SampleApi.Consumer.Handlers.Customers.Interceptors
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Octokit;
    using Innovation.Api.Interceptors;

    using ApiSample.Customers.Commands;

    public class InsertCustomerInterceptor : ICommandInterceptor<InsertCustomer>
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

        public async Task Intercept(InsertCustomer command)
        {
            await this.HandleIntercept(command: command);
        }

        #endregion Methods

        #region Private Methods

        private async Task HandleIntercept(InsertCustomer command)
        {
            try
            {
                var github = new GitHubClient(new ProductHeaderValue("Innovation"));
                var user = await github.User.Get(command.Criteria.UserName);

                var exists = user != null;

                command.Criteria.SetGithubStatus(existsOnGithub: exists);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);

                command.Criteria.SetGithubStatus(existsOnGithub: false);
            }
        }

        #endregion Private Methods
    }
}
