
using System.Threading.Tasks;
using Del.Common.Requests;
using Del.Common.Responses;

namespace Del.Common.Services
{
    public interface IApiservice
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);

        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
    }
}
