using System;
using lineBot.Core.Interfaces;

namespace lineBot.Core.Dto
{
    [Serializable]
    public class SendTextMessageDto : IMessage
    {
        public string Type => "text";
        public string Text { get; set; }
    }
}
