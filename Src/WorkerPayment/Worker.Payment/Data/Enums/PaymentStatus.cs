namespace WorkerPayment.Data.Enums;

public enum PaymentStatus
{
    Aprovado,
    Recusado,       // Cartão sem saldo, CVV inválido, etc.
    ErroTecnico,    // Gateway fora do ar (Fallback acionado)
}
