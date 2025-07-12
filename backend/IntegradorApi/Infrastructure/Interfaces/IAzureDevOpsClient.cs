using System.Threading.Tasks;

namespace IntegradorApi.Infrastructure.Interfaces
{
    public interface IAzureDevOpsClient
    {
        Task<int> ClonarWorkItemAsync(
           string origemUrl, string patOrigem,
           string destinoUrl, string patDestino,
           string projetoDestino, string iterationPath,
           string atividade, string responsavel,
           int idWorkItemOrigem);
    }
}
