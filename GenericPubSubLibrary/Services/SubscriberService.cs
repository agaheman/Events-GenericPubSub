using GenericPubSubLibrary.Interfaces;
using GenericPubSubLibrary.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;

namespace GenericPubSubLibrary.Services
{
    public class SubscriberService<TSubscriber> where TSubscriber : class, IRabbitSubscriber, new()
	{
		private readonly ConcurrentDictionary<Guid, TSubscriber> _consumers = new ConcurrentDictionary<Guid, TSubscriber>();

		public void Get(MessagingConfiguration messagingConfiguration, IRabbitMessageHandler messageHandler)
		{
			using (var messageConsumer = new TSubscriber())
			{
				messageConsumer.Get(messagingConfiguration,messageHandler);
			}
		}

		public Guid Subscribe(MessagingConfiguration messagingConfiguration, IRabbitMessageHandler messageHandler)
		{
			var consumer = new TSubscriber();
			var consumerId = Guid.NewGuid();
			_consumers.TryAdd(consumerId, consumer);
			consumer.Consume(messagingConfiguration, messageHandler);

			return consumerId;
		}

		public void Unsubscribe(Guid consumerId)
		{
			var consumer = _consumers[consumerId];
			consumer.Dispose();
			_consumers.TryRemove(consumerId, out TSubscriber _);
		}
	}
}
