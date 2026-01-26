using Newtonsoft.Json;

namespace PARAGCore.Clients.Models.Serialization
{
    internal class PARAGJSONSerializer
    {
        static PARAGJSONSerializer()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MaxDepth = 64
            };

            _settings = jsonSerializerSettings;
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        private static JsonSerializerSettings _settings;
    }
}
