using WorkerShipping.Data.Enums;

namespace WorkerShipping.Dtos.Response;

public record EnvioResponse(ShippingStatus Status, string TrackingCode, string Mensagem);
