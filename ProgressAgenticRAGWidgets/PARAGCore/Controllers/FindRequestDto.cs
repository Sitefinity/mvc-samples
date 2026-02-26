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
        public string[] Show { get; set; } = new[] { "basic", "values", "origin" };

        [JsonPropertyName("top_k")]
        public int Take { get; set; } = 20;

        [JsonPropertyName("search_configuration")]
        public string ConfigurationName { get; set; }
    }
}
