using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace NativeChatWidget.Configuration
{
    public class NativeChatConfig : ConfigSection
    {
        private const string ApiKeyPropName = "ApiKey";
        private const string ApiEndPointPropName = "ApiEndpoint";

        [ObjectInfo(typeof(NativeChatResources), Title = "ApiKey", Description = "ApiKeyDescription")]
        [ConfigurationProperty(ApiKeyPropName)]
        [SecretData]
        public string ApiKey
        {
            get
            {
                return (string)this[ApiKeyPropName];
            }

            set
            {
                this[ApiKeyPropName] = value;
            }
        }

        [ObjectInfo(typeof(NativeChatResources), Title = "ApiEndPointTitle", Description = "ApiEndPointDescription")]
        [ConfigurationProperty(ApiEndPointPropName, DefaultValue = "https://api.nativechat.com/v1/")]
        public string NativeChatApiEndpoint
        {
            get
            {
                return (string)this[ApiEndPointPropName];
            }

            set
            {
                this[ApiEndPointPropName] = value;
            }
        }
    }
}
