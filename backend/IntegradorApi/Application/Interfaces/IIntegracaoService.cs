using System.Threading.Tasks;
using IntegradorApi.Application.Models.Requests;
using IntegradorApi.Application.Models.Responses;

namespace IntegradorApi.Application.Interfaces
{
    public interface IIntegracaoService
    {
        Task<ClonarTaskResponse> ClonarAsync(int idWorkItemOrigem);
    }
}
