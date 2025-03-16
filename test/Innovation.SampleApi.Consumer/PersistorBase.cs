namespace Innovation.SampleApi.Consumer
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using Innovation.Api.Commanding;
    using Innovation.Api.CommandHelpers;

    public abstract class PersistorBase<T> : ICommandHandler<T> where T : class, ICommand
    {
        #region Fields

        private readonly ILogger logger;
        private readonly CommandResult commandResult;

        private ICommand command;

        #endregion Fields

        #region Constructor

        public PersistorBase(ILogger<PersistorBase<T>> logger)
        {
            this.logger = logger;
            this.commandResult = new CommandResult();
        }

        #endregion Constructor

        #region Methods

        public async ValueTask<ICommandResult> Handle(T command)
        {
            try
            {
                this.command = command;

                return await this.Persist();
            }
            catch(Exception ex)
            {
                this.logger.LogError(exception: ex, message: ex.GetInnerMostMessage());
                this.commandResult.Fail(ex: ex);

                return this.commandResult;
            }
        }

        public ICommandResult ReturnError(string errorMessage)
        {
            this.commandResult.Fail(errorMessage: errorMessage);

            return this.commandResult;
        }

        public ICommandResult ReturnSuccess(Guid recordId)
        {
            this.commandResult.SetRecord(recordId: recordId);

            return this.commandResult;
        }

        public abstract Task<ICommandResult> Persist();

        #endregion Methods
    }
}
