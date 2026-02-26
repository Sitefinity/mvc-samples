using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PARAGCore.Controllers
{
    public class AskRequestDto
    {
        [JsonPropertyName("knowledgeBoxName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "KB Id is required.")]
        public string KnowledgeBoxName { get; set; }

        [JsonPropertyName("query")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Query is required.")]
        [RegularExpression(@"^.{1,20000}$", ErrorMessage = "Query is too long.")]
        public string Query { get; set; }

        [JsonPropertyName("chat_history")]
        public List<InteractionDto> ChatHistory { get; set; } = new List<InteractionDto>();

        [JsonPropertyName("search_configuration")]
        public string ConfigurationName { get; set; }

        [JsonPropertyName("citations")]
        public string ShowSources { get; set; }

        [JsonPropertyName("show")]
        public string[] ShowOptions { get; set; }
    }
}
