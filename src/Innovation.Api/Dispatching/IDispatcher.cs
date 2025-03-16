namespace Innovation.Api.Dispatching
{
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;

    using Querying;
    using Messaging;
    using Commanding;

    /// <summary>
    /// This is the interface that a dispatcher must implement.
    /// It is the type that user code will request when they want to dispatch an event.
    /// </summary>
    public interface IDispatcher
    {
        void SetCorrelationId([DisallowNull] string correlationId);
        void SetContext([DisallowNull] IDispatcherContext dispatcherContext);
        ValueTask<ICommandResult> Command<TCommand>([DisallowNull] TCommand command, bool suppressExceptions = true) where TCommand : ICommand;
        Task Message<TMessage>([DisallowNull] TMessage message) where TMessage : IMessage;
        Task MessageFor<TMessage>([DisallowNull] TMessage message, params string[] addresses) where TMessage : IMessage;
        Task<TQueryResult> Query<TQuery, TQueryResult>([DisallowNull] TQuery query)
            where TQueryResult : IQueryResult
            where TQuery : IQuery;
        Task<TQueryResult> QueryFor<TQuery, TQueryResult>([DisallowNull] TQuery query, params string[] addresses)
            where TQueryResult : IQueryResult
            where TQuery : IQuery;
    }
}