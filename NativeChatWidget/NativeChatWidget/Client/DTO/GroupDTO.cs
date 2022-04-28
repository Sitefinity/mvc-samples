using System.Collections.Generic;
using Newtonsoft.Json;

namespace NativeChatWidget.Client.DTO
{
    public class GroupDTO
    {
        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Value { get; set; }

        [JsonProperty("expressions")]
        public List<string> Expressions { get; set; }

        [JsonProperty("answers")]
        public List<string> Answers { get; set; }

        [JsonProperty("ssmlAnswers", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public List<string> SsmlAnswers { get; set; }
    }
}
