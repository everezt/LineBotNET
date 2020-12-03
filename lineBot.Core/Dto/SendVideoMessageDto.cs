using lineBot.Core.Interfaces;

namespace lineBot.Core.Dto
{
    public class SendVideoMessageDto : IMessage
    {
        public string Type => "video";
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
    }
}
