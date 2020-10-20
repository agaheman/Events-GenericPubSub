using GenericPubSubLibrary.Interfaces;
using GenericPubSubLibrary.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace GenericPubSubLibrary.Services
{
	public class PublisherService<TPublisher> where TPublisher : class, IRabbitPublisher, new()
	{
		private readonly ConnectionFactory _connectionFactory;

		public PublisherService(RabbitOptions rabbitOptions)
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
			var messagePublisher = new TPublisher();
			messagePublisher.Publish(payloads,messagingConfiguration);
		}
	}
}