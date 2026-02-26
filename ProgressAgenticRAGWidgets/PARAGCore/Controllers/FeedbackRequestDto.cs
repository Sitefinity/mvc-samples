using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PARAGCore.Controllers
{
    public class FeedbackRequestDto
    {
        [JsonPropertyName("knowledgeBoxName")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "KB Id is required.")]
        public string KnowledgeBoxName { get; set; }

        [JsonPropertyName("ident")]
        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }

        [JsonPropertyName("good")]
        [Required(ErrorMessage = "Is good value is required")]
        public bool IsGood { get; set; }

        [JsonPropertyName("feedback")]
        public string Feedback { get; set; }

        [JsonPropertyName("task")]
        public string Task { get; set; }
    }
}
