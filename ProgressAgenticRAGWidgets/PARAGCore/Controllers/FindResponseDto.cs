using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PARAGCore.Controllers
{
    public class FindResponseDto
    {
        [JsonPropertyName("resources")]
        public Dictionary<string, AgenticRAGClientResources> Resources { get; set; }
    }

    public class AgenticRAGClientResources
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("origin")]
        public Origin Origin { get; set; }

        [JsonPropertyName("fields")]
        public Dictionary<string, Field> Fields { get; set; }
    }

    public class Origin
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Field
    {
        [JsonPropertyName("paragraphs")]
        public Dictionary<string, Paragraph> Paragraphs { get; set; }
    }

    public class Paragraph
    {
        [JsonPropertyName("order")]
        public int Order { get; set; }
    }
}
