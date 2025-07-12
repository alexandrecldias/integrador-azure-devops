using IntegradorApi.Application.Interfaces;
using IntegradorApi.Application.Models.Responses;
using IntegradorApi.Infrastructure.AzureDevOps;
using IntegradorApi.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace IntegradorApi.Application.Services
{
    public class IntegracaoService : IIntegracaoService
    {
        private readonly IAzureDevOpsClient _devOpsClient;
        private readonly IConfiguration _config;
        private readonly IParametroService _parametroService;


        public IntegracaoService(IAzureDevOpsClient devOpsClient, IConfiguration config, IParametroService parametroService)
        {
            _devOpsClient = devOpsClient;
            _config = config;
            _parametroService = parametroService;
        }

        public async Task<ClonarTaskResponse> ClonarAsync(int idWorkItemOrigem)
        {
            // Aqui você pode buscar as configs do banco, por enquanto do appsettings
            var origemUrl = (await _parametroService.ObterPorChaveAsync("ORIGEM_URL"))?.Valor;
            var destinoUrl = (await _parametroService.ObterPorChaveAsync("DESTINO_URL"))?.Valor;
            var projetoDestino = (await _parametroService.ObterPorChaveAsync("PROJETO_DESTINO"))?.Valor;
            var patOrigem = (await _parametroService.ObterPorChaveAsync("PAT_ORIGEM"))?.Valor;
            var patDestino = (await _parametroService.ObterPorChaveAsync("PAT_DESTINO"))?.Valor;
            var iterationPath = (await _parametroService.ObterPorChaveAsync("ITERATION_PATH"))?.Valor;
            var atividade = (await _parametroService.ObterPorChaveAsync("ATIVIDADE_TASK"))?.Valor;
            var responsavel = (await _parametroService.ObterPorChaveAsync("RESPONSAVEL_TASK"))?.Valor;

            int taskCriada = await _devOpsClient.ClonarWorkItemAsync(
                origemUrl, patOrigem, destinoUrl, patDestino,
                projetoDestino, iterationPath, atividade, responsavel, idWorkItemOrigem
            );

            return new ClonarTaskResponse
            {
                IdTaskCriada = taskCriada,
                Mensagem = taskCriada > 0 ? "Task clonada com sucesso." : "Falha ao clonar task."
            };
        }
    }
}
