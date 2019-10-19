namespace Innovation.SampleApi.Consumer.Stores
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Innovation.Api.Core;
    using Innovation.Api.Querying;
    using Innovation.Api.Messaging;
    using Innovation.Api.Commanding;
    using Innovation.Api.Dispatching;

    public class SampleAuditStore : IAuditStore
    {
        #region Fields

        private readonly Dictionary<string, List<IEvent>> eventStream;

        #endregion Fields

        #region Constructor

        public SampleAuditStore()
        {
            this.eventStream = new Dictionary<string, List<IEvent>>();
        }

        #endregion Constructor

        #region Properties

        public Dictionary<string, List<IEvent>> EventStream => this.eventStream;

        #endregion Properties

        #region Methods

        public Task Log(string correlationId, ICommand command, ICommandResult commandResult)
        {
            this.InsertEvent(correlationId: correlationId, @event: command);

            return Task.FromResult(0);
        }

        public Task Log(string correlationId, IQuery query)
        {
            this.InsertEvent(correlationId: correlationId, @event: query);

            return Task.FromResult(0);
        }

        public Task Log(string correlationId, IMessage message)
        {
            this.InsertEvent(correlationId: correlationId, @event: message);

            return Task.FromResult(0);
        }

        #endregion Methods

        #region Private Methods

        private void InsertEvent(string correlationId, IEvent @event)
        {
            if (this.eventStream.ContainsKey(key: correlationId))
            {
                var existingEntry = this.eventStream[key: correlationId];
                existingEntry.Add(item: @event);
            }
            else
            {
                this.eventStream.Add(key: correlationId, value: new List<IEvent> { @event });
            }
        }

        #endregion Private Methods
    }
}
