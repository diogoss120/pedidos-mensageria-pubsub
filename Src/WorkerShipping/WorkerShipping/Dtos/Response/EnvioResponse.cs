namespace WorkerShipping.Dtos.Response;

public record EnvioResponse(bool Sucesso, string TrackingCode, string Mensagem);
