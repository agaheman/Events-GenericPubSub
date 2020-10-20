namespace GenericPubSubLibrary.Models
{
	public class Payload
	{
		public string Label { get; set; }
		public string MessageId { get; set; }
		public string CorrelationId { get; set; }
		public string ReplyTo { get; set; }
		public object Body { get; set; }
	}
}
