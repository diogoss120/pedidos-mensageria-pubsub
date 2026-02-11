
namespace WorkerNotification.Configuration;

public class PubSubConfig
{
    public const string SectionName = "PubSubConfig";

    public string ProjectId { get; set; } = string.Empty;
    public string SubscriptionId { get; set; } = string.Empty;
}