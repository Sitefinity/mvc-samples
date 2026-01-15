using System.Configuration;
using Telerik.Sitefinity.Configuration;

namespace PARAGCore.Configuration
{
    public class AssistantConfig : ConfigElement
    {
        private const string AdminApiBaseUrlPropertyName = "adminApiBaseUrl";
        private const string CdnHostNamePropertyName = "cdnHostName";
        private const string CdnRootFolderRelativePathPropertyName = "cdnRootFolderRelativePath";

        public AssistantConfig(ConfigElement parent)
            : base(parent)
        {
        }

        [ConfigurationProperty(AdminApiBaseUrlPropertyName)]
        public string AdminApiBaseUrl
        {
            get
            {
                return (string)this[AdminApiBaseUrlPropertyName];
            }

            set
            {
                this[AdminApiBaseUrlPropertyName] = value;
            }
        }

        [ConfigurationProperty(CdnHostNamePropertyName)]
        public string CdnHostName
        {
            get
            {
                return (string)this[CdnHostNamePropertyName];
            }

            set
            {
                this[CdnHostNamePropertyName] = value;
            }
        }

        [ConfigurationProperty(CdnRootFolderRelativePathPropertyName)]
        public string CdnRootFolderRelativePath
        {
            get
            {
                return (string)this[CdnRootFolderRelativePathPropertyName];
            }

            set
            {
                this[CdnRootFolderRelativePathPropertyName] = value;
            }
        }
    }
}
