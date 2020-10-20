using GenericPubSubLibrary.Interfaces;
using GenericPubSubLibrary.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace GenericPubSubLibrary.Services
{
    public class DefaultSubscriberService : IRabbitSubscriber
    {
        private IModel _channel;
        private IConnection _connection;
        private string _consumerTag;
        private EventingBasicConsumer _consumer;
        private readonly ConnectionFactory _connectionFactory;

        public DefaultSubscriberService(RabbitOptions rabbitOptions)
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

        public void Get(MessagingConfiguration messagingConfiguration, IRabbitMessageHandler messageHandler)
        {
            SetupChannel(messagingConfiguration);

            var result = _channel.BasicGet(messagingConfiguration.QueueName, true);
            while (result != null)
            {
                messageHandler.Handle(result);
                result = _channel.BasicGet(messagingConfiguration.QueueName, true);
            }

        }
        public void Consume(MessagingConfiguration messagingConfiguration, IRabbitMessageHandler messageHandler)
        {
            SetupChannel(messagingConfiguration);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, result) =>
            {
                try
                {
                    _channel.BasicAck(result.DeliveryTag, false);
                    messageHandler.Handle(model, result);
                }
                catch (Exception ex)
                {
                    //Log exception
                }
            };

            _consumerTag = _channel.BasicConsume(messagingConfiguration.QueueName, false, _consumer);
        }

        private void SetupChannel(MessagingConfiguration messagingConfiguration)
        {
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(messagingConfiguration.ExchangeName, messagingConfiguration.Type, messagingConfiguration.Durable, messagingConfiguration.AutoDelete);
            _channel.QueueDeclare(messagingConfiguration.QueueName, messagingConfiguration.Durable, false, messagingConfiguration.AutoDelete, null);
            _channel.QueueBind(messagingConfiguration.QueueName, messagingConfiguration.ExchangeName, messagingConfiguration.RoutingKey);
        }
        public void Dispose()
        {
            if (_channel?.IsOpen == true && _consumer != null)
            {
                _channel.BasicCancel(_consumerTag);
            }

            _channel?.Dispose();
            _connection?.Dispose();
        }
    }

}
