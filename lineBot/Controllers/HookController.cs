using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lineBot.Core;
using lineBot.Core.Dto;
using lineBot.Core.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace lineBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HookController : ControllerBase
    {

        private static List<string> _events = new List<string>();
        private readonly ILogger<HookController> _logger;
        private readonly IMessageService _msgService;
        private readonly LineBotConfig _config;

        public HookController(ILogger<HookController> logger,
            IMessageService msgService,
            LineBotConfig config)
        {
            _logger = logger;
            _msgService = msgService;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> MessageHook([FromBody] MessageHookDto hook)
        {
            await _msgService.AnalyseMessageAndRespond(hook);

            _events.Add(JsonConvert.SerializeObject(hook));

            if (_events.Count > 10)
            {
                _events = _events.TakeLast(10).ToList();
            }

            return Ok(hook);
        }

        [HttpGet]
        [Route("log")]
        public IEnumerable<string> Log()
        {
            return _events;
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            await _msgService.SendRandomGiphy("C5432fabbc9e27c44262b17d9ead6caa6", "ass");
            return Ok(_config.Endpoint);
        }

        [HttpGet]
        [Route("inter")]
        public async Task<IActionResult> Inter()
        {

            var asd = new SendMessageDto
            {
                To = "Random"
            };

            var msg = new SendImageMessageDto
            {
                PreviewImageUrl = "pre",
                OriginalContentUrl = "original"
            };

            asd.Messages = new[] { msg };

            if (asd.Messages.FirstOrDefault() is SendImageMessageDto)
            {
                asd.To = "found it";
            }

            var res = JsonConvert.SerializeObject(asd);

            return Ok(res);
        }
    }
}
