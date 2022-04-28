using Newtonsoft.Json;

namespace NativeChatWidget.Client.DTO
{
    public class NativeChatChannelConfig
    {
        [JsonProperty("authToken")]
        public string AuthToken { get; set; }
    }
}
