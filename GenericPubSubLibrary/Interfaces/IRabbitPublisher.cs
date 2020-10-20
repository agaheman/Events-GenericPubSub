using GenericPubSubLibrary.Models;
using System.Collections.Generic;

namespace GenericPubSubLibrary.Interfaces
{
    public interface IRabbitPublisher
    {
        void Publish(IEnumerable<Payload> payloads, MessagingConfiguration messagingConfiguration);
        void Publish(Payload payload, MessagingConfiguration messagingConfiguration);
    }
}
