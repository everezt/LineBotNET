using System.Threading.Tasks;
using lineBot.Core.Dto;
using lineBot.Core.ServiceInterfaces.ApplicationServices;

namespace lineBot.Core.ServiceInterfaces
{
    public interface IMessageService : IApplicationService
    {
        Task AnalyseMessageAndRespond(MessageHookDto dto);
        Task SendRandomGiphy(string groupId, params string[] args);
    }
}
