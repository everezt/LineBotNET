using System.Threading.Tasks;
using lineBot.Core.ServiceInterfaces.ApplicationServices;

namespace lineBot.Core.ServiceInterfaces
{
    public interface IRestApiService : IApplicationService
    {
        Task SendToLineAsync<T>(T item, string url);
        Task<TRes> GetAsync<TRes>(string url);
    }
}
