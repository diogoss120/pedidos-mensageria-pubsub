namespace WorkerPayment.Dtos.Request
{
    public record PagamentoDto(
        string NumeroCartao,
        string Titular,
        string Validade,
        string Cvv,
        decimal Valor);
}
