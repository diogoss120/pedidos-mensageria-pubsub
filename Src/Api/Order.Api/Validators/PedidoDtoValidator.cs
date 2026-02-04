using FluentValidation;
using Order.Api.Dtos;

namespace Order.Api.Validators
{
    public class PedidoDtoValidator : AbstractValidator<PedidoDto>
    {
        public PedidoDtoValidator()
        {
            // Valida o objeto de Cliente usando o ClienteDtoValidator
            RuleFor(x => x.Cliente)
                .NotNull().WithMessage("Os dados do cliente são obrigatórios.")
                .SetValidator(new ClienteDtoValidator());

            // Valida se a lista de itens não está vazia e valida cada item individualmente
            RuleFor(x => x.Itens)
                .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.");

            RuleForEach(x => x.Itens)
                .SetValidator(new ItemPedidoDtoValidator());

            // Valida o objeto de Pagamento usando o PagamentoDtoValidator
            RuleFor(x => x.Pagamento)
                .NotNull().WithMessage("Os dados de pagamento são obrigatórios.")
                .SetValidator(new PagamentoDtoValidator());
        }
    }
}
