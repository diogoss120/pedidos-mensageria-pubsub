using FluentValidation;
using Order.Api.Dtos;

namespace Order.Api.Validators
{
    public class PagamentoDtoValidator : AbstractValidator<PagamentoDto>
    {
        public PagamentoDtoValidator()
        {
            RuleFor(x => x.NumeroCartao)
                .NotEmpty().WithMessage("O número do cartão é obrigatório.")
                .CreditCard().WithMessage("Número de cartão de crédito inválido.");

            RuleFor(x => x.Titular)
                .NotEmpty().WithMessage("O nome do titular é obrigatório.");

            RuleFor(x => x.Validade)
                .NotEmpty().WithMessage("A validade é obrigatória.")
                .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$")
                .WithMessage("A validade deve estar no formato MM/AA.");

            RuleFor(x => x.Cvv)
                .NotEmpty().WithMessage("O CVV é obrigatório.")
                .Matches(@"^\d{3,4}$").WithMessage("O CVV deve conter 3 ou 4 dígitos.");
        }
    }
}
