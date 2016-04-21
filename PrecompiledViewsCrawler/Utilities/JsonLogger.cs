using System.IO;
using System.Linq;
using System.Web.Hosting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrecompiledViewsCrawler.Utilities
{
    internal static class JsonLogger
    {
        static JsonLogger()
        {
            Result = new JArray();
        }

        public static void AddToLog(JObject obj)
        {
            var oldObj = Result.FirstOrDefault(x => x["viewPath"].ToString() == obj["viewPath"].ToString() && x["url"].ToString() == obj["url"].ToString());
            if (oldObj == null)
            {
                Result.Add(obj);
                return;
            }

            Result.Remove(oldObj);
            Result.Add(obj);
        }

        public static void SaveToFile(string virtualPath = JsonLogger.LogVirtualPath)
        {
            string absolutePath = HostingEnvironment.MapPath(virtualPath);
            using (StreamWriter file = File.CreateText(absolutePath))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.WriteRaw(BeautifyResult(Result));
            }
        }

        private static string BeautifyResult(JArray result)
        {
            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        private static readonly JArray Result;
        private const string LogVirtualPath = "~/App_Data/Sitefinity/Logs/PrecompilationStatistics.json";
    }
}