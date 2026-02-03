using System.Collections.Generic;

namespace PARAGCore.OperationProviders
{
    public class SuggestionsDto
    {
        public EntitiesDto Entities { get; set; }

        public ParagraphsDto Paragraphs { get; set; }
    }

    public class EntitiesDto
    {
        public IList<Entity> Entities { get; set; }
    }

    public class Entity
    {
        public string Value { get; set; }
    }

    public class ParagraphsDto
    {
        public IList<ResultDto> Results { get; set; }
    }

    public class ResultDto
    {
        public string Text { get; set; }
    }
}
