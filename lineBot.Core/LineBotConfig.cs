namespace lineBot.Core
{
    public class LineBotConfig
    {
        public string Endpoint { get; set; }
        public string AccessToken { get; set; }
        public string MasterUserId { get; set; }
        public GiphyConfig Giphy { get; set; }
    }

    public class GiphyConfig
    {
        public string Endpoint { get; set; }
        public string ApiKey { get; set; }
    }
}
