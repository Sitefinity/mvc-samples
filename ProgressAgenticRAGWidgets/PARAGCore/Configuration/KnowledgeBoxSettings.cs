using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace PARAGCore.Configuration
{
    public class KnowledgeBoxSettings : ConfigElement
    {
        public KnowledgeBoxSettings(ConfigElement parent)
            : base(parent)
        {
        }

        [ObjectInfo(Title = "Base Url")]
        [ConfigurationProperty(PropertyNames.BaseUrl)]
        public string BaseUrl
        {
            get
            {
                return (string)this[PropertyNames.BaseUrl];
            }

            set
            {
                this[PropertyNames.BaseUrl] = value;
            }
        }

        [ObjectInfo(Title = "Knowledge box name")]
        [ConfigurationProperty(PropertyNames.KnowledgeBoxName, IsRequired = true, IsKey = true, DefaultValue = "")]
        public string KnowledgeBoxName
        {
            get
            {
                return (string)this[PropertyNames.KnowledgeBoxName];
            }

            set
            {
                this[PropertyNames.KnowledgeBoxName] = value;
            }
        }

        [ObjectInfo(Title = "Knowledge box UID")]
        [ConfigurationProperty(PropertyNames.KnowledgeBoxId, IsRequired = true, DefaultValue = "")]
        public string KnowledgeBoxId
        {
            get
            {
                return (string)this[PropertyNames.KnowledgeBoxId];
            }

            set
            {
                this[PropertyNames.KnowledgeBoxId] = value;
            }
        }

        [ObjectInfo(Title = "Knowledge box API key")]
        [ConfigurationProperty(PropertyNames.KnowledgeBoxToken)]
        [SecretData]
        public string KnowledgeBoxToken
        {
            get
            {
                return (string)this[PropertyNames.KnowledgeBoxToken];
            }

            set
            {
                this[PropertyNames.KnowledgeBoxToken] = value;
            }
        }

        private static class PropertyNames
        {
            public const string KnowledgeBoxId = "knowledgeBoxId";
            public const string KnowledgeBoxToken = "knowledgeBoxToken";
            public const string KnowledgeBoxName = "knowledgeBoxName";
            public const string BaseUrl = "baseUrl";
        }
    }
}
