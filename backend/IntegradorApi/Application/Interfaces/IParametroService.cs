using IntegradorApi.Application;
using IntegradorApi.Domain;

namespace IntegradorApi.Application.Interfaces;

public interface IParametroService
{
    Task<List<Parametro>> ObterTodosAsync();
    Task<Parametro?> ObterPorChaveAsync(string chave);
    Task<bool> AtualizarValorAsync(string chave, string novoValor);
}
