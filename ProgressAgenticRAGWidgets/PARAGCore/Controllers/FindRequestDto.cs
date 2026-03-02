using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PARAGCore.Controllers
{
    public class FindRequestDto
    {
        [JsonPropertyName("knowledgeBoxName")]
        public string KnowledgeBoxName { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("show")]
        public string[] Show { get; set; }

        [JsonPropertyName("top_k")]
        [JsonProperty("top_k")]
        public int Take { get; set; }

        [JsonPropertyName("search_configuration")]
        [JsonProperty("search_configuration")]
        public string ConfigurationName { get; set; }
    }
}
