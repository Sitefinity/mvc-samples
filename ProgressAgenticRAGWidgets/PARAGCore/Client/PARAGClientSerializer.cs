using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace PARAGCore.Clients.Models.Serialization
{
    internal class PARAGJSONSerializer
    {
        static PARAGJSONSerializer()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = null,
                // ensures Dictionary keys are serialized as is. 
                DictionaryKeyPolicy = null,
                MaxDepth = 64,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };

            _options = jsonSerializerOptions;
        }

        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }

        private static JsonSerializerOptions _options;
    }
}
