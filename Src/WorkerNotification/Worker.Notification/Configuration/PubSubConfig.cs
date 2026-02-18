
namespace WorkerNotification.Configuration;

public class PubSubConfig
{
    public const string SectionName = "PubSubConfig";

    public string ProjectId { get; set; } = string.Empty;
    public string SubscriptionIdPedidoCriado { get; set; } = string.Empty;
    public string SubscriptionIdPagamentoProcessado { get; set; } = string.Empty;
    public string SubscriptionIdPedidoDespachado { get; set; } = string.Empty;
}