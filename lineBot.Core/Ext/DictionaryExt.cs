using System.Collections.Generic;
using System.Linq;

namespace lineBot.Core.Ext
{
    public static class DictionaryExt
    {
        public static string ToArgs(this Dictionary<string, string> dict)
        {
            var pairs = dict.Select(x => $"{x.Key}={x.Value}");
            return $"?{string.Join("&", pairs)}";
        }
    }
}
