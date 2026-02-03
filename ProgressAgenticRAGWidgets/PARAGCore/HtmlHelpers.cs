using PARAGCore.OperationProviders;
using System;
using System.Net.Http;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;

namespace PARAGCore
{
    public class HtmlHelpers
    {
        private const string AdminApiBaseUrlPropertyName = "adminApiBaseUrl";
        private const string CdnHostNamePropertyName = "cdnHostName";
        private const string CdnRootFolderRelativePathPropertyName = "cdnRootFolderRelativePath";

        public static string GetCdnUrl(string cdnFile)
        {
            string cdnUrlFormatString = BuildCdnUrlFormatString();
            return string.Format(cdnUrlFormatString, cdnFile);
        }

        internal static string BuildCdnUrlFormatString()
        {
            var manager = ConfigManager.GetManager();
            var section = manager.GetSection("AgenticRAGConfig");
            var assistantProp = section["assistant"] as ConfigElement;

            string version = null;
            try
            {
                var versionInfo = RetrieveVersionInfo((string)assistantProp[AdminApiBaseUrlPropertyName]);
                version = versionInfo?.ProductVersion;
            }
            catch (Exception ex)
            {
                string logMessage = $"Error retrieving assistant version info. Please check the assistant configuration details: {ex.Message}";
                Log.Write(logMessage, ConfigurationPolicy.Trace);
            }

            string cdnHostName = (string)assistantProp[CdnHostNamePropertyName];
            string cdnRootFolderRelativePath = (string)assistantProp[CdnRootFolderRelativePathPropertyName];
            string rootRelativePath = cdnRootFolderRelativePath == null ?
                "staticfiles/" :
                (string.IsNullOrEmpty(cdnRootFolderRelativePath) ? string.Empty : $"{cdnRootFolderRelativePath.Trim('/')}/");
            string versionSuffix = string.IsNullOrEmpty(version) ? string.Empty : $"?ver={version}";
            string placeholder = "{0}";

            return $"https://{cdnHostName}/{rootRelativePath}{placeholder}{versionSuffix}";
        }

        private static VersionInfoDto RetrieveVersionInfo(string adminAPIBaseUrl)
        {
            var pathUrl = "/Version";
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri(new Uri(adminAPIBaseUrl),
                pathUrl)
            );

            var httpResponseMessage = new HttpClient().SendAsync(request).GetAwaiter().GetResult();
            httpResponseMessage.EnsureSuccessStatusCode();

            var result = httpResponseMessage.Content.ReadAsAsync<VersionInfoDto>().GetAwaiter().GetResult();

            return result;
        }
    }
}
