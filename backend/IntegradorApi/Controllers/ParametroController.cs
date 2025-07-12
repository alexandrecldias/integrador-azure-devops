using IntegradorApi.Application;
using IntegradorApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntegradorApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParametroController : ControllerBase
{
    private readonly IParametroService _service;

    public ParametroController(IParametroService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var parametros = await _service.ObterTodosAsync();
        return Ok(parametros);
    }

    [HttpGet("{chave}")]
    public async Task<IActionResult> Get(string chave)
    {
        var parametro = await _service.ObterPorChaveAsync(chave);
        if (parametro == null) return NotFound();

        return Ok(parametro);
    }

    [HttpPut("{chave}")]
    public async Task<IActionResult> Put(string chave, [FromBody] AtualizarParametroRequest request)
    {
        var sucesso = await _service.AtualizarValorAsync(chave, request.Valor);
        if (!sucesso)
            return NotFound(new { mensagem = "Parâmetro não encontrado" });

        return Ok(new { mensagem = "Parâmetro atualizado com sucesso" });
    }
}
