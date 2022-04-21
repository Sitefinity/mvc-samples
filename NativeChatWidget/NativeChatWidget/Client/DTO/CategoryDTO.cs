using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NativeChatWidget.Client.DTO
{
    public class CategoryDTO
    {
        public string Name { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
        public LookupStrategy LookupStrategy { get; set; }
    }

    public enum LookupStrategy
    {
        QnA,
        trait,
        keywords
    }
}
