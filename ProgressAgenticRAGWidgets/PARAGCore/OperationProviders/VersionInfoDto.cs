using System.Runtime.Serialization;

namespace PARAGCore.OperationProviders
{
    public class VersionInfoDto
    {
        [DataMember]
        public string ProductVersion { get; set; }
    }
}
