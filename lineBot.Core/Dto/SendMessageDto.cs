using System;
using System.Collections.Generic;
using lineBot.Core.Interfaces;

namespace lineBot.Core.Dto
{
    [Serializable]
    public class SendMessageDto
    {
        public string To { get; set; }
        public IEnumerable<IMessage> Messages { get; set; }
    }
}
