using Shared.Data.Entities;

namespace WorkerNotification.Data.Entities;

public class Notification : EntityBase
{

    public Guid PedidoId { get; set; }
    public string? ClienteNome { get; set; }
    public string? ConteudoEmail { get; set; }
    public DateTime DataEnvio { get; set; }
}
