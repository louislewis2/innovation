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

        public Task Log(AuditContext auditContext, ICommand command, ICommandResult commandResult)
        {
            this.InsertEvent(auditContext: auditContext, @event: command);

            return Task.CompletedTask;
        }

        public Task Log(AuditContext auditContext, IQuery query)
        {
            this.InsertEvent(auditContext: auditContext, @event: query);

            return Task.CompletedTask;
        }

        public Task Log(AuditContext auditContext, IMessage message)
        {
            this.InsertEvent(auditContext: auditContext, @event: message);

            return Task.CompletedTask;
        }

        #endregion Methods

        #region Private Methods

        private void InsertEvent(AuditContext auditContext, IEvent @event)
        {
            if (this.eventStream.ContainsKey(key: auditContext.CorrelationId))
            {
                var existingEntry = this.eventStream[key: auditContext.CorrelationId];
                existingEntry.Add(item: @event);
            }
            else
            {
                this.eventStream.Add(key: auditContext.CorrelationId, value: new List<IEvent> { @event });
            }
        }

        #endregion Private Methods
    }
}
