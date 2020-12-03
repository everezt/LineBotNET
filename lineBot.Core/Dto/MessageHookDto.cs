
using System.Collections.Generic;

namespace lineBot.Core.Dto
{
    public class MessageHookDto
    {
        public IEnumerable<MessageHookBaseDto> Events { get; set; }
        public string Destination { get; set; }
    }

    public class MessageHookBaseDto
    {
        public string Type { get; set; }
        public string ReplyToken { get; set; }
        public MessageHookSourceDto Source { get; set; }
        public ulong Timestamp { get; set; }
        public string Mode { get; set; }
        public MessageHookMessageDto Message { get; set; }
    }

    public class MessageHookSourceDto
    {
        public string GroupId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
    }

    public class MessageHookMessageDto
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
