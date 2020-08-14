namespace Innovation.Api.Dispatching
{
    using System.Threading.Tasks;

    using Querying;
    using Messaging;
    using Commanding;

    /// <summary>
    /// This is the interface that a dispatcher must implement.
    /// It is the type that user code will request when they want to dispatch an event.
    /// </summary>
    public interface IDispatcher
    {
        void SetCorrelationId(string correlationId);
        void SetContext(IDispatcherContext dispatcherContext);
        Task<ICommandResult> Command<TCommand>(TCommand command, bool suppressExceptions = true) where TCommand : ICommand;
        Task Message<TMessage>(TMessage message) where TMessage : IMessage;
        Task MessageFor<TMessage>(TMessage message, params string[] addresses) where TMessage : IMessage;
        Task<TQueryResult> Query<TQuery, TQueryResult>(TQuery query)
            where TQueryResult : IQueryResult
            where TQuery : IQuery;
        Task<TQueryResult> QueryFor<TQuery, TQueryResult>(TQuery query, params string[] addresses)
            where TQueryResult : IQueryResult
            where TQuery : IQuery;

        bool CanCommand<TCommand>(TCommand command) where TCommand : ICommand;
        bool CanMessage<TMessage>(TMessage message) where TMessage : IMessage;
        bool CanMessageFor<TMessage>(TMessage message, params string[] addresses) where TMessage : IMessage;
        bool CanQuery<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery where TQueryResult : IQueryResult;
        bool CanQueryFor<TQuery, TQueryResult>(TQuery query, params string[] addresses) where TQuery : IQuery where TQueryResult : IQueryResult;
    }
}