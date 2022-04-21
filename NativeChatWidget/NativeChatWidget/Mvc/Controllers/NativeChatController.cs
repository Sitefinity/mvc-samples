using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using NativeChatWidget.Mvc.Models;
using NativeChatWidget.Renderer;
using Progress.Sitefinity.Renderer.Designers;
using Progress.Sitefinity.Renderer.Designers.Attributes;
using Progress.Sitefinity.Renderer.Entities.Content;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Personalization;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web.UI;

namespace NativeChatWidget.Mvc.Controllers
{
    [ControllerToolboxItem(
        Name = "NativeChatWidget_MVC",
        Title = nameof(NativeChatResources.NativeChat),
        ResourceClassId = nameof(NativeChatResources),
        SectionName = "Feather samples",
        CssClass = WidgetIconCssClass)]
    [Localization(typeof(NativeChatResources))]
    public class NativeChatController : Controller, ICustomWidgetVisualizationExtended, IPersonalizable
    {
        private const string WidgetIconCssClass = "sfForumsViewIcn sfMvcIcn";
        private string googleApiKey;
        private string locale;

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chatbot", 0)]
        [DisplayName("Select a chatbot")]
        [DataType(customDataType: KnownFieldTypes.Choices)]
        [ExternalDataChoice]
        public string BotId { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chatbot", 1)]
        [DisplayName("Nickname of the bot")]
        [Description("Name displayed before bot's messages in the chat.")]
        public string Nickname { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chatbot", 2)]
        [DisplayName("Avatar of the bot")]
        [Content(Type = "Telerik.Sitefinity.Libraries.Model.Image", AllowMultipleItemsSelection = false, LiveData = false)]
        public MixedContentContext BotAvatar { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chatbot", 3)]
        [DisplayName("Conversation trigger expression")]
        [Description("You can customize the bot's initial conversation by adding a phrase on a specific topic. The bot will skip the general introduction and start with questions directly related to this topic.")]
        [DataType(customDataType: "textArea")]
        public string UserMessage { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chat window", 0)]
        [DisplayName("Chat window mode")]
        [DataType(customDataType: KnownFieldTypes.RadioChoice)]
        public ChatWindowMode ChatMode { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chat window", 1)]
        [DisplayName("Opening chat icon")]
        [Description("Select a custom icon for opening chat window. If left empty, default icon will be displayed.")]
        [Content(Type = "Telerik.Sitefinity.Libraries.Model.Image", AllowMultipleItemsSelection = false)]
        [ConditionalVisibility("{\"conditions\":[{\"fieldName\":\"ChatMode\",\"operator\":\"Equals\",\"value\":\"modal\"}]}")]
        public MixedContentContext OpeningChatIcon { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chat window", 2)]
        [DisplayName("Closing chat icon")]
        [Description("Select a custom icon for closing chat window. If left empty, default icon will be displayed.")]
        [Content(Type = "Telerik.Sitefinity.Libraries.Model.Image", AllowMultipleItemsSelection = false)]
        [ConditionalVisibility("{\"conditions\":[{\"fieldName\":\"ChatMode\",\"operator\":\"Equals\",\"value\":\"modal\"}]}")]
        public MixedContentContext ClosingChatIcon { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chat window", 3)]
        [DisplayName("Container ID")]
        [Description("ID of the HTML element that will host the chat widget.")]
        [ConditionalVisibility("{\"conditions\":[{\"fieldName\":\"ChatMode\",\"operator\":\"Equals\",\"value\":\"inline\"}]}")]
        public string ContainerId { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Message box", 0)]
        [DisplayName("Placeholder text in the message box")]
        [DefaultValue("Type a message...")]
        public string Placeholder { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Message box", 1)]
        [DisplayName("Show...")]
        [DefaultValue(ChatPickers.FilePicker | ChatPickers.LocationPicker)]
        [Choice("[{\"Title\":\"FilePicker\",\"Name\":\"File Picker\",\"Value\":1,\"Icon\":null},{\"Title\":\"LocationPicker\",\"Name\":\"Location Picker\",\"Value\":2,\"Icon\":null}]")]
        public ChatPickers ShowPickers { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Message box", 2)]
        [DisplayName("Button label for location")]
        [Description("Submit button text used in location picker that can be popped from send message area of widget.")]
        [DefaultValue("Submit")]
        [ConditionalVisibility("{\"operator\":\"Or\",\"conditions\":[{\"fieldName\":\"ShowPickers\",\"operator\":\"Equals\",\"value\":\"01\"},{\"fieldName\":\"ShowPickers\",\"operator\":\"Equals\",\"value\":\"11\"}]}")]
        public string LocationPickerLabel { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Message box", 3)]
        [DisplayName("Google API key")]
        [ConditionalVisibility("{\"operator\":\"Or\",\"conditions\":[{\"fieldName\":\"ShowPickers\",\"operator\":\"Equals\",\"value\":\"01\"},{\"fieldName\":\"ShowPickers\",\"operator\":\"Equals\",\"value\":\"11\"}]}")]
        public string GoogleApiKey
        {
            get
            {
                if (this.googleApiKey == null)
                {
                    var apiKey = Config.Get<SystemConfig>().GeoLocationSettings.GoogleMapApiV3Key;
                    if (!string.IsNullOrEmpty(apiKey))
                    {
                        this.googleApiKey = apiKey;
                    }
                }

                return this.googleApiKey;
            }
            set
            {
                this.googleApiKey = value;
            }
        }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Message box", 4)]
        [DisplayName("Default latitude and longitude")]
        [Description("Used to center the location picker in case the user declines the prompt to allow geolocation in the browser.")]
        [ConditionalVisibility("{\"operator\":\"Or\",\"conditions\":[{\"fieldName\":\"ShowPickers\",\"operator\":\"Equals\",\"value\":\"01\"},{\"fieldName\":\"ShowPickers\",\"operator\":\"Equals\",\"value\":\"11\"}]}")]
        public string DefaultLocation { get; set; }

        [Category("Advanced")]
        [DisplayName("CSS for custom design")]
        [Placeholder("type URL or path to file...")]
        public string CustomCss { get; set; }

        [Category("Advanced")]
        [DisplayName("Locale")]
        [Description("Currently supported major locales by NativeChat: ‘en’, ‘ar’, ‘pt’, ‘de’, ‘es’, ‘fi’, ‘bg’, ‘it’, ‘nl’, ‘hr’.")]
        public string Locale
        {
            get
            {
                if (this.locale == null)
                {
                    var currentCulture = SystemManager.CurrentContext.Culture;
                    var culture = currentCulture.IsNeutralCulture ? currentCulture : currentCulture.Parent;

                    this.locale = culture.Name;
                }

                return this.locale;
            }
            set
            {
                this.locale = value;
            }
        }

        [Browsable(false)]
        public string EmptyLinkText
        {
            get
            {
                return Res.Get<NativeChatResources>().EmptyWidgetText;
            }
        }

        /// <inheritdoc />
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return BotId.IsNullOrEmpty();
            }
        }

        /// <summary>
        /// Gets the widget CSS class.
        /// </summary>
        /// <value>
        /// The widget CSS class.
        /// </value>
        [Browsable(false)]
        public string WidgetCssClass
        {
            get
            {
                return WidgetIconCssClass;
            }
        }

        public ActionResult Index()
        {
            var viewModel = new NativeChatViewModel(
                BotId,
                Nickname,
                BotAvatar,
                UserMessage,
                ChatMode,
                Placeholder,
                ShowPickers,
                OpeningChatIcon,
                ClosingChatIcon,
                ContainerId,
                LocationPickerLabel,
                GoogleApiKey,
                CustomCss,
                Locale,
                DefaultLocation);
            return View("Index", viewModel);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
        }
    }

    public enum ChatWindowMode
    {
        [Description("Display modal")]
        modal,
        [Description("Display inline")]
        inline
    }

    [Flags]
    public enum ChatPickers
    {
        [Description("File picker")]
        FilePicker = 0x01,
        [Description("Location picker")]
        LocationPicker = 0x02
    }
}
