using GenericPubSubLibrary;
using GenericPubSubLibrary.Models;
using GenericPubSubLibrary.Models.Rabbit;
using GenericPubSubLibrary.Services;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace SubscriberApp
{
    class Program
    {
        public static IConfiguration Configuration;

        public static void Main(string[] args)
        {

            Configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.RabbitMq.json", optional: false, reloadOnChange: true)
           .AddCommandLine(args)
           .Build();

            var rabbitOptions = new RabbitOptions();
            Configuration.GetSection("RabbitMqConnection").Bind(rabbitOptions);
            var subscriber = new DefaultSubscriberService(rabbitOptions);

            var messageHandler = new RabbitMessageHandler();
            var messagingConfiguration = new MessagingConfiguration(exchangeName: "WeatherForecastExchange", queueName: "WeatherForecast-Queue");
            
            subscriber.Consume(messagingConfiguration, messageHandler);
            
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }


    }
}
