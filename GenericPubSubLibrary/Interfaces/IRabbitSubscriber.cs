using GenericPubSubLibrary.Models;
using System;

namespace GenericPubSubLibrary.Interfaces
{
    public interface IRabbitSubscriber : IDisposable
    {
        void Get(MessagingConfiguration messagingConfiguration, IRabbitMessageHandler messageHandler);
        void Consume(MessagingConfiguration messagingConfiguration, IRabbitMessageHandler messageHandler);
    }
}