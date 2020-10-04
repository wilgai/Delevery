
using System.Threading.Tasks;
using Del.Common.Responses;

namespace Del.Common.Services
{
    public interface IApiservice
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
