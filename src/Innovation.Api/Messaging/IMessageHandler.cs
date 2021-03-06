﻿namespace Innovation.Api.Messaging
{
    using System.Threading.Tasks;

    /// <summary>
    /// This is the interface all message handlers must implement.
    /// </summary>
    public interface IMessageHandler<in TMessage> where TMessage : IMessage
    {
        /// <summary>
        /// This is the method that will handle the issued message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task Handle(TMessage message);
    }
}
