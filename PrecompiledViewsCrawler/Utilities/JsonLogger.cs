using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace PrecompiledViewsCrawler.Utilities
{
    public class JsonLogger : IJsonLogger
    {
        // TODO: Refactor file write

        public void SaveToFile(object data, string fileName)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            string path = this.MapLogFilePath(fileName);
            bool shouldPlaceComma = true;

            if (!File.Exists(path))
            {
                shouldPlaceComma = false;
                using (File.Create(path)) { }
            }

            using (StreamWriter file = File.AppendText(path))
            {
                if (shouldPlaceComma)
                {
                    file.WriteLine(Comma);
                }

                file.Write(json);
            }
        }

        private string MapLogFilePath(string fileName)
        {
            return HostingEnvironment.MapPath(LogDirectory + fileName);
        }

        private const string LogDirectory = "~/App_Data/Sitefinity/Logs/";
        private const string Comma = ",";
    }
}
