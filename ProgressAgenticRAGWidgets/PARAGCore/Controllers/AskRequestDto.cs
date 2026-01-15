using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PARAGCore.Controllers
{
    public class AskRequestDto
    {
        [JsonProperty("knowledgeBoxName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "KB Id is required.")]
        public string KnowledgeBoxName { get; set; }

        [JsonProperty("query")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Query is required.")]
        [RegularExpression(@"^.{1,20000}$", ErrorMessage = "Query is too long.")]
        public string Query { get; set; }

        [JsonProperty("chat_history")]
        public List<InteractionDto> ChatHistory { get; set; } = new List<InteractionDto>();

        [JsonProperty("search_configuration")]
        public string ConfigurationName { get; set; }

        [JsonProperty("citations")]
        public string ShowSources { get; set; }

        [JsonProperty("show")]
        public string[] ShowOptions { get; set; }
    }
}
