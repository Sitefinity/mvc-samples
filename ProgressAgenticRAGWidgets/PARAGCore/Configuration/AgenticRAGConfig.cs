using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace PARAGCore.Configuration
{
    public class AgenticRAGConfig : ConfigSection
    {
        [ObjectInfo(Title = "AccountId")]
        [ConfigurationProperty(PropertyNames.AccountId)]
        public string AccountId
        {
            get
            {
                return (string)this[PropertyNames.AccountId];
            }

            set
            {
                this[PropertyNames.AccountId] = value;
            }
        }

        [ObjectInfo(Title = "NUA Key")]
        [ConfigurationProperty(PropertyNames.NuaKey)]
        [SecretData]
        public string NuaKey
        {
            get
            {
                return (string)this[PropertyNames.NuaKey];
            }

            set
            {
                this[PropertyNames.NuaKey] = value;
            }
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

        [ObjectInfo(Title = "Knowledge boxes")]
        [ConfigurationProperty(PropertyNames.KnowledgeBoxes)]
        public virtual ConfigElementDictionary<string, KnowledgeBoxSettings> KnowledgeBoxes
        {
            get
            {
                return (ConfigElementDictionary<string, KnowledgeBoxSettings>)this[PropertyNames.KnowledgeBoxes];
            }
        }

        [ConfigurationProperty(PropertyNames.Assistant)]
        public virtual AssistantConfig AssistantConfig
        {
            get
            {
                return (AssistantConfig)this[PropertyNames.Assistant];
            }
        }

        private static class PropertyNames
        {
            public const string KnowledgeBoxes = "knowledgeBoxes";
            public const string NuaKey = "nuaKey";
            public const string AccountId = "accountId";
            public const string BaseUrl = "baseUrl";
            public const string Assistant = "assistant";
        }
    }
}
