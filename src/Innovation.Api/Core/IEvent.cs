namespace Innovation.Api.Core
{
    public interface IEvent
    {
        /// <summary>
        /// Name is a friendly name for each event
        /// </summary>
        string EventName { get; }
    }
}
