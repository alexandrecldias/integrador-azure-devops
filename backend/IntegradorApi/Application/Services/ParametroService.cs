using IntegradorApi.Application.Interfaces;
using IntegradorApi.Domain;
using IntegradorApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntegradorApi.Application.Services;

public class ParametroService : IParametroService
{
    private readonly AppDbContext _context;

    public ParametroService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Parametro>> ObterTodosAsync()
    {
        return await _context.Parametros.ToListAsync();
    }

    public async Task<Parametro?> ObterPorChaveAsync(string chave)
    {
        return await _context.Parametros.FirstOrDefaultAsync(p => p.Chave == chave);
    }

    public async Task<bool> AtualizarValorAsync(string chave, string novoValor)
    {
        var parametro = await _context.Parametros.FirstOrDefaultAsync(p => p.Chave == chave);
        if (parametro == null)
            return false;

        parametro.Valor = novoValor;
        await _context.SaveChangesAsync();
        return true;
    }
}
