namespace WorkerShipping.Configuration
{
    public class PubSubConfig
    {
        public const string SectionName = "PubSubConfig";

        public string ProjectId { get; set; } = string.Empty;
        public string SubscriptionId { get; set; } = string.Empty;
        public string TopicId { get; set; } = string.Empty;
    }

}
