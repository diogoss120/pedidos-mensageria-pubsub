using FluentValidation;
using Order.Api.Dtos;

namespace Order.Api.Validators
{
    public class ItemPedidoDtoValidator : AbstractValidator<ItemPedidoDto>
    {
        public ItemPedidoDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do item é obrigatório.");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

            RuleFor(x => x.Preco)
                .GreaterThan(0).WithMessage("O preço deve ser um valor positivo.");
        }
    }
}
