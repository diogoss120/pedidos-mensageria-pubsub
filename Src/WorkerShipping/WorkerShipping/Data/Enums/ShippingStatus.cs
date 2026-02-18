namespace WorkerShipping.Data.Enums;

public enum ShippingStatus
{
    Despachado,         // Sucesso: etiqueta gerada e código de rastreio atribuído
    ErroTecnico,      // Falha após todas as tentativas do Polly (Fallback)
    Processando       // Envio em processamento (Idempotência)
}
