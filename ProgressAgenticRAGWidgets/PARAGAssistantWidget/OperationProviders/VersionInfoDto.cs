using System.Runtime.Serialization;

namespace PARAGAssistantWidget.OperationProviders
{
    internal class VersionInfoDto
    {
        [DataMember]
        public string ProductVersion { get; set; }
    }
}
