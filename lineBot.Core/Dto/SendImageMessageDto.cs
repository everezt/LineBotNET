using System;
using lineBot.Core.Interfaces;

namespace lineBot.Core.Dto
{
    [Serializable]
    public class SendImageMessageDto : IMessage
    {
        public string Type => "image";
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
    }
}
