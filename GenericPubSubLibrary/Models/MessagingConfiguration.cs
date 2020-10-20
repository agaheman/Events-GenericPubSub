namespace GenericPubSubLibrary.Models
{
    public class MessagingConfiguration
    {
        /// <summary>
        /// Publish
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="type"></param>
        /// <param name="durable"></param>
        /// <param name="autoDelete"></param>
        public MessagingConfiguration(string exchangeName, string routingKey = "", string type = "fanout", bool durable = false, bool autoDelete = false)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
        }

        /// <summary>
        /// Subscriber
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="routingKey"></param>
        /// <param name="type"></param>
        /// <param name="durable"></param>
        /// <param name="autoDelete"></param>
        public MessagingConfiguration(string exchangeName, string queueName, string routingKey = "", string type = "fanout", bool durable = false, bool autoDelete = false)
        {
            ExchangeName = exchangeName;
            QueueName = queueName;
            RoutingKey = routingKey;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
        }

        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
        public string Type { get; set; }
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
    }
}
