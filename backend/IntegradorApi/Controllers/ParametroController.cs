using Microsoft.AspNetCore.Mvc;
using IntegradorApi.Persistence;
using IntegradorApi.Application;
using Microsoft.EntityFrameworkCore;

namespace IntegradorApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParametroController : ControllerBase
{
    private readonly AppDbContext _context;

    public ParametroController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{chave}")]
    public IActionResult Get(string chave)
    {
        var param = _context.Parametros.FirstOrDefault(p => p.Chave == chave);
        if (param == null) return NotFound();

        return Ok(param);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.Parametros.ToList());
    }

    [HttpPut("{chave}")]
    public async Task<IActionResult> Put(string chave, [FromBody] AtualizarParametroRequest request)
    {
        var parametro = await _context.Parametros.FirstOrDefaultAsync(p => p.Chave == chave);
        if (parametro == null)
            return NotFound(new { mensagem = "Parâmetro não encontrado" });

        parametro.Valor = request.Valor;

        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Parâmetro atualizado com sucesso", parametro });
    }

}
