namespace Order.Api.Dtos
{
    public class PagamentoDto
    {
        public string NumeroCartao { get; set; } = string.Empty;

        public string Titular { get; set; } = string.Empty;

        public string Validade { get; set; } = string.Empty;

        public string Cvv { get; set; } = string.Empty;
    }
}
