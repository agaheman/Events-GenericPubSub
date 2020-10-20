using GenericPubSubLibrary.Interfaces;
using GenericPubSubLibrary.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenericPubSubLibrary.Services
{
    public class DefaultPublisherService : IRabbitPublisher
    {
        private readonly ConnectionFactory _connectionFactory;
        public DefaultPublisherService(RabbitOptions rabbitOptions)
        {
            _connectionFactory = new ConnectionFactory()
            {
                UserName = rabbitOptions.UserName,
                Password = rabbitOptions.UserName,
                HostName = rabbitOptions.HostName,
                Port = rabbitOptions.Port,
                VirtualHost = rabbitOptions.VHost
            };
        }

        public void Publish(IEnumerable<Payload> payloads, MessagingConfiguration messagingConfiguration)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(messagingConfiguration.ExchangeName, messagingConfiguration.Type, messagingConfiguration.Durable, messagingConfiguration.AutoDelete, null);

                foreach (var payload in payloads)
                {
                    var properties = channel.CreateBasicProperties();
                    properties.CorrelationId = payload.CorrelationId ?? "";
                    properties.ReplyTo = payload.ReplyTo ?? "";
                    var bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload.Body));

                    try
                    {
                        channel.BasicPublish(messagingConfiguration.ExchangeName, messagingConfiguration.RoutingKey, true, properties, bodyBytes);
                    }
                    catch (Exception ex)
                    {
                        //Log exception
                    }
                }
            }
        }

        public void Publish(Payload payload, MessagingConfiguration messagingConfiguration)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(messagingConfiguration.ExchangeName, messagingConfiguration.Type, messagingConfiguration.Durable, messagingConfiguration.AutoDelete, null);

                var properties = channel.CreateBasicProperties();
                properties.CorrelationId = payload.CorrelationId ?? "";
                properties.ReplyTo = payload.ReplyTo ?? "";
                var bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload.Body));

                try
                {
                    channel.BasicPublish(messagingConfiguration.ExchangeName, messagingConfiguration.RoutingKey, true, properties, bodyBytes);
                }
                catch (Exception ex)
                {
                    //Log exception
                }

            }
        }

    }
}
