using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using lineBot.Core;
using lineBot.Core.Dto;
using lineBot.Core.ServiceInterfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace lineBot.ApplicationServices
{
    public class RestApiService : IRestApiService
    {
        private readonly LineBotConfig _config;

        public RestApiService(LineBotConfig config)
        {
            _config = config;
        }

        public async Task SendToLineAsync<T>(T item, string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.AccessToken);
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                var json = new StringContent(JsonConvert.SerializeObject(item, serializerSettings), Encoding.UTF8, "application/json");
                var res = await client.PostAsync(url, json);
                Console.Write(res.IsSuccessStatusCode);
            }
        }

        public async Task<TRes> GetAsync<TRes>(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var str = await response.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<TRes>(str);
                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                return (TRes)default;
            }

            return (TRes)default;
        }
    }
}
