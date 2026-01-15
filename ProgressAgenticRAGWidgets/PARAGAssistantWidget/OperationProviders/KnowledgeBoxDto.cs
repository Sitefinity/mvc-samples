using System.Runtime.Serialization;

namespace PARAGAssistantWidget.OperationProviders
{
    [DataContract]
    public class KnowledgeBoxDto
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}
