using System.Text.Json.Serialization;

namespace PARAGCore.Controllers
{
    public class FeedbackRequestDto
    {
        [JsonPropertyName("knowledgeBoxName")]
        public string KnowledgeBoxName { get; set; }

        [JsonPropertyName("ident")]
        public string Ident { get; set; }

        [JsonPropertyName("good")]
        public bool Good { get; set; }

        [JsonPropertyName("feedback")]
        public string Feedback { get; set; }

        [JsonPropertyName("task")]
        public string Task { get; set; }
    }
}
