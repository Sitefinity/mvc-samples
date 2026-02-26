using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace PARAGCore.Configuration
{
    public class AgenticRAGConfig : ConfigSection
    {
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
            public const string Assistant = "assistant";
        }
    }
}
