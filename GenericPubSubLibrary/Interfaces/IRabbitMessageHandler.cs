using GenericPubSubLibrary.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;

namespace GenericPubSubLibrary.Interfaces
{
    public interface IRabbitMessageHandler
    {
        List<Payload> Payloads { get; set; }
        void Handle(BasicGetResult result);
        void Handle(object model, BasicDeliverEventArgs result);
    }
}
