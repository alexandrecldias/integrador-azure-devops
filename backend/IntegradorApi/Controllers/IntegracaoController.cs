using IntegradorApi.Application.Interfaces;
using IntegradorApi.Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IntegradorApi.Controllers
{
    public class IntegracaoController : Controller
    {
        private readonly IIntegracaoService _integracaoService;

        public IntegracaoController(IIntegracaoService integracaoService)
        {
            _integracaoService = integracaoService;
        }

        [HttpPost("clonar-task")]
        public async Task<IActionResult> Clonar([FromBody] ClonarTaskRequest request)
        {
            var resultado = await _integracaoService.ClonarAsync(request.IdWorkItemOrigem);
            return Ok(resultado);
        }
    }
}
