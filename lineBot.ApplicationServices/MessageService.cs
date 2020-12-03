using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using lineBot.Core;
using lineBot.Core.Dto;
using lineBot.Core.Ext;
using lineBot.Core.Interfaces;
using lineBot.Core.ServiceInterfaces;
using Newtonsoft.Json.Linq;

namespace lineBot.ApplicationServices
{
    public class MessageService : IMessageService
    {
        private readonly LineBotConfig _config;
        private readonly IRestApiService _apiService;

        public MessageService(LineBotConfig config,
            IRestApiService apiService)
        {
            _config = config;
            _apiService = apiService;
        }
        
        private string[] _allowedJokeCategories =
        {
            "Programming",
            "Miscellaneous",
            "Dark",
            "Pun",
            "Spooky",
            "Christmas"
        };

        public async Task AnalyseMessageAndRespond(MessageHookDto dto)
        {
            foreach (var evnt in dto.Events)
            {
                bool isAdmin = evnt.Source.UserId == _config.MasterUserId;

                var split = evnt.Message.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var cmd = split.FirstOrDefault();
                var args = split.Skip(1).ToArray();

                if (evnt.Message.Type == "text")
                {
                    // admin commands
                    if (isAdmin)
                    {
                        if (string.Equals(evnt.Message.Text, "hi", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (!string.IsNullOrEmpty(evnt.Source.GroupId))
                            {
                                await SendMessage(evnt.Source.GroupId, PrepareTextMessage("Hi, daddy iiv!"));
                            }
                        }
                    }

                    if (evnt.Message.Text.StartsWith("!"))
                    {
                        switch (cmd.Trim())
                        {
                            case "!joke":
                                var joke = await GetJoke(args);
                                if (!joke.Error)
                                {
                                    await SendMessage(evnt.Source.GroupId, PrepareTextMessage(joke.Joke));
                                }
                                break;

                            case "!giphy":
                                await SendRandomGiphy(evnt.Source.GroupId, args);
                                break;
                        }
                    }

                }
            }
        }

        private async Task SendMessage(string to, IMessage msg)
        {
            var res = new SendMessageDto
            {
                To = to,
                Messages = new []{msg}
            };

            await _apiService.SendToLineAsync(res, "https://api.line.me/v2/bot/message/push");
        }

        private SendTextMessageDto PrepareTextMessage(string msg)
        {
            return new SendTextMessageDto
            {
                Text = msg
            };
        }

        private async Task<JokeDto> GetJoke(params string[] args)
        {
            var hasArgs = args.Any();
            string category = string.Empty;

            if (hasArgs && _allowedJokeCategories.Any(x => string.Equals(x, args[0].Trim(), StringComparison.InvariantCultureIgnoreCase)))
            {
                category = args[0];
            }

            var reqUrl = $"https://sv443.net/jokeapi/v2/joke/{(!string.IsNullOrEmpty(category) ? category : "Any")}?type=single";

            return await _apiService.GetAsync<JokeDto>(reqUrl);
        }


        public async Task SendRandomGiphy(string groupId, params string[] args)
        {
            var query = new Dictionary<string, string>();

            if (args.Any())
            {
                query.Add("tag", string.Join(" ", args));
            }
            else
            {
                await SendMessage(groupId, PrepareTextMessage("Invalid usage.Use: !giphy something"));
            }

            query.Add("apiKey", _config.Giphy.ApiKey);
            query.Add("rating", "r");

            var req = $"{_config.Giphy.Endpoint}{query.ToArgs()}";
            var res = await _apiService.GetAsync<JObject>(req);

            var originalUrl = res["data"]?["images"]?["original"]?["mp4"]?.ToObject<string>();
            var previewUrl = res["data"]?["images"]?["original_still"]?["url"]?.ToObject<string>();

            if (!string.IsNullOrEmpty(originalUrl) && !string.IsNullOrEmpty(previewUrl))
            {
                var msg = new SendVideoMessageDto
                {
                    OriginalContentUrl = originalUrl,
                    PreviewImageUrl = previewUrl
                };

                await SendMessage(groupId, msg);
            }
        }
    }
}
