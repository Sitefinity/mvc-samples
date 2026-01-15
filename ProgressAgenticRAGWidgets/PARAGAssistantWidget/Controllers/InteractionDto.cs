using Newtonsoft.Json;

namespace PARAGAssistantWidget.Controllers
{
    public class InteractionDto
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
