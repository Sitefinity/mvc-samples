using Telerik.Sitefinity.Localization;

namespace NativeChatWidget
{
    internal class NativeChatResources : Resource
    {
        [ResourceEntry("ConfigurationPageTitle",
            Value = "Connector for NativeChat",
            Description = "Title of the NativeChat configuration page",
            LastModified = "2021/04/06")]
        public string ConfigurationPageTitle
        {
            get
            {
                return this["ConfigurationPageTitle"];
            }
        }

        [ResourceEntry("ConfigurationPageUrlName",
            Value = "Configuration",
            Description = "The url name of the NativeChat configuration page",
            LastModified = "2021/04/06")]
        public string ConfigurationPageUrlName
        {
            get
            {
                return this["ConfigurationPageUrlName"];
            }
        }

        [ResourceEntry("ApiKey",
            Value = "API key",
            Description = "API key",
            LastModified = "2021/11/08")]
        public string ApiKey
        {
            get
            {
                return this["ApiKey"];
            }
        }

        [ResourceEntry("ApiKeyDescription",
            Value = "Used to connect to NativeChat services",
            Description = "API key description",
            LastModified = "2021/04/06")]
        public string ApiKeyDescription
        {
            get
            {
                return this["ApiKeyDescription"];
            }
        }

        [ResourceEntry("ApiKeyHint",
            Value = "Go to your NativeChat account and generate an API token. Copy the token to your clipboard and use it here.",
            Description = "API key hint",
            LastModified = "2021/04/06")]
        public string ApiKeyHint
        {
            get
            {
                return this["ApiKeyHint"];
            }
        }

        [ResourceEntry("ConfigurationPageSubtitle",
            Value = "Connect using your NativeChat credentials",
            Description = "Configuration page subtitle",
            LastModified = "2021/04/06")]
        public string ConfigurationPageSubtitle
        {
            get
            {
                return this["ConfigurationPageSubtitle"];
            }
        }

        [ResourceEntry("NativeChat",
            Value = "NativeChat",
            Description = "NativeChat",
            LastModified = "2021/04/06")]
        public string NativeChat
        {
            get
            {
                return this["NativeChat"];
            }
        }

        [ResourceEntry("OperationFailed",
            Value = "OperationFailed",
            Description = "OperationFailed",
            LastModified = "2021/04/06")]
        public string OperationFailed
        {
            get
            {
                return this["OperationFailed"];
            }
        }

        [ResourceEntry("DisconnectDialogMessage",
            Value = "Disconnecting NativeChat will cause chatbots on your site to stop working. </br> </br> Do you want to proceed?",
            Description = "DisconnectDialogMessage",
            LastModified = "2021/04/06")]
        public string DisconnectDialogMessage
        {
            get
            {
                return this["DisconnectDialogMessage"];
            }
        }

        [ResourceEntry("EmptyWidgetText",
            Value = "Select a chatbot",
            Description = "EmptyWidgetText",
            LastModified = "2021/04/06")]
        public string EmptyWidgetText
        {
            get
            {
                return this["EmptyWidgetText"];
            }
        }

        [ResourceEntry("ApiEndPointTitle",
            Value = "NativeChat API endpoint URL",
            Description = "Phrase: NativeChat API endpoint",
            LastModified = "2021/11/08")]
        public string ApiEndPointTitle
        {
            get
            {
                return this["ApiEndPointTitle"];
            }
        }

        [ResourceEntry("ApiEndPointDescription",
            Value = "The URL to the NativeChat REST APIs.",
            Description = "Phrase: The URL to the NativeChat REST APIs.",
            LastModified = "2021/11/08")]
        public string ApiEndPointDescription
        {
            get
            {
                return this["ApiEndPointDescription"];
            }
        }
    }
}
