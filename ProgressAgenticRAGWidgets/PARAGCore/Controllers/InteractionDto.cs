using System.Text.Json.Serialization;

namespace PARAGCore.Controllers
{
    public class InteractionDto
    {
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
