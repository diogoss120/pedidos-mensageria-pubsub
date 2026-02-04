using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Dtos;
using Order.Api.Services.Interfaces;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IValidator<PedidoDto> _validator;
        private readonly IPedidoService _pedidoService;

        public PedidosController(IValidator<PedidoDto> validator, IPedidoService pedidoService)
        {
            _validator = validator;
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] PedidoDto pedido)
        {
            var result = _validator.Validate(pedido);

            if(!result.IsValid)
            {
                var erros = result.Errors.Select(e => new {e.PropertyName, e.ErrorMessage});
                return BadRequest(erros);
            }

            await _pedidoService.ProcessarPedido(pedido);

            return Accepted();
        }
    }
}
