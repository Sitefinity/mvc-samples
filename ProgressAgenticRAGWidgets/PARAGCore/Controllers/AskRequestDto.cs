using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PARAGCore.Controllers
{
    public class AskRequestDto
    {
        [JsonPropertyName("knowledgeBoxName")]
        public string KnowledgeBoxName { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonProperty("chat_history")]
        [JsonPropertyName("chat_history")]
        public List<InteractionDto> ChatHistory { get; set; } = new List<InteractionDto>();

        [JsonProperty("search_configuration")]
        [JsonPropertyName("search_configuration")]
        public string ConfigurationName { get; set; }

        [JsonPropertyName("citations")]
        public string Citations { get; set; }

        [JsonPropertyName("show")]
        public string[] Show { get; set; }
    }
}
