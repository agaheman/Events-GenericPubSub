using GenericPubSubLibrary.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericPubSubLibrary.Models.Rabbit
{
    public class RabbitMessageHandler : IRabbitMessageHandler
    {
        public List<Payload> Payloads { get; set; } = new List<Payload>();

        public void Handle(BasicGetResult result)
        {
            Payloads.Add(new Payload()
            {
                Body = Encoding.UTF8.GetString(result.Body.ToArray()),
                MessageId = result.BasicProperties.MessageId,
                CorrelationId = result.BasicProperties.CorrelationId,
                ReplyTo = result.BasicProperties.ReplyTo
            });
        }

        public void Handle(object model, BasicDeliverEventArgs result)
        {
            var payload = new Payload()
            {
                Body = Encoding.UTF8.GetString(result.Body.ToArray()),
                MessageId = result.BasicProperties.MessageId,
                CorrelationId = result.BasicProperties.CorrelationId,
                ReplyTo = result.BasicProperties.ReplyTo
            };
            Payloads.Add(payload);

            Console.WriteLine(payload.Body);
        }
    }
}
