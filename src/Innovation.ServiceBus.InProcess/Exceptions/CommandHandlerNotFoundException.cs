namespace Innovation.ServiceBus.InProcess.Exceptions
{
    using System;

    using Api.Commanding;

    public class CommandHandlerNotFoundException : Exception
    {
        #region Fields

        private readonly ICommand command;

        #endregion Fields

        #region Constructor

        public CommandHandlerNotFoundException(ICommand command) : base($"Command Handler not Found. Command.Name: {command.EventName}; Command.Type: {command.GetType()}")
        {
            this.command = command;
        }

        #endregion Constructor

        #region Properties

        public ICommand Command => this.command;
        public string CommandName => this.command.EventName;
        public Type CommandType => this.command.GetType();

        #endregion Properties
    }
}