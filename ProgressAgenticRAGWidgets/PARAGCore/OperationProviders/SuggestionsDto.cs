using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PARAGCore.OperationProviders
{
    public class SuggestionsDto
    {
        [JsonPropertyName("entities")]
        public EntitiesDto Entities { get; set; }

        [JsonPropertyName("paragraphs")]
        public ParagraphsDto Paragraphs { get; set; }
    }

    public class EntitiesDto
    {
        [JsonPropertyName("entities")]
        public IList<Entity> Entities { get; set; }
    }

    public class Entity
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class ParagraphsDto
    {
        [JsonPropertyName("results")]
        public IList<ResultDto> Results { get; set; }
    }

    public class ResultDto
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
