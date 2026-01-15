using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PARAGAssistantWidget.Controllers
{
    public class FeedbackRequestDto
    {
        [JsonProperty("knowledgeBoxName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "KB Id is required.")]
        public string KnowledgeBoxName { get; set; }

        [JsonProperty("ident")]
        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }

        [JsonProperty("good")]
        [Required(ErrorMessage = "Is good value is required")]
        public bool IsGood { get; set; }

        [JsonProperty("feedback")]
        public string Feedback { get; set; }

        [JsonProperty("task")]
        public string Task { get; set; }
    }
}
