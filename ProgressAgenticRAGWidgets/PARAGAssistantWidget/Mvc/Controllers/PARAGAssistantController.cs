using PARAGCore.Configuration;
using PARAGAssistantWidget.Mvc.Models;
using PARAGCore.OperationProviders;
using Progress.Sitefinity.Renderer.Designers;
using Progress.Sitefinity.Renderer.Designers.Attributes;
using Progress.Sitefinity.Renderer.Entities.Content;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Mvc;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Personalization;
using Telerik.Sitefinity.Web.UI;

namespace PARAGAssistantWidget.Mvc.Controllers
{
    [ControllerToolboxItem(
        Name = "PARAGAssistant_MVC",
        Title = "PARAG Assistant",
        SectionName = "Marketing",
        CssClass = WidgetIconCssClass)]
    public class PARAGAssistantController : Controller, ICustomWidgetVisualizationExtended, IPersonalizable
    {
        private const string WidgetIconCssClass = "sfForumsViewIcn sfMvcIcn";
        private readonly HttpClient httpClient;

        public PARAGAssistantController()
        {
            this.httpClient = new HttpClient();
        }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("AI assistant", 1)]
        [DisplayName("Agentic RAG connection")]
        [Description("[{\"Type\":1,\"Chunks\":[{\"Value\":\"A connection to a specific knowledge box in Preogress Agentic RAG. Select which connection this widget should use to search and answer questions.\",\"Presentation\":[]}]},{\"Type\":1,\"Chunks\":[{\"Value\":\"Manage connections in \",\"Presentation\":[]},{\"Value\":\"Administration > Progress Agentic Rag connections\",\"Presentation\":[3]}]}]")]
        [DataType(customDataType: KnownFieldTypes.Choices)]
        [Choice(ServiceUrl = "/Default.GetConfiguredKnowledgeBoxes()", ServiceWarningMessage = "No PARAG knowledge boxes are found.")]
        [Placeholder("Select connection")]
        public string KnowledgeBoxName { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("AI assistant", 2)]
        [DisplayName("Search configuration")]
        [Description("[{\"Type\":1,\"Chunks\":[{\"Value\":\"A saved set of search settings that the AI uses to find content.\",\"Presentation\":[]}]},{\"Type\":1,\"Chunks\":[{\"Value\":\"Can be found in Progress Agentic Rag portal \",\"Presentation\":[]},{\"Value\":\"Search > Saved configurations\",\"Presentation\":[3]}]}]")]
        public string ConfigurationName { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("AI assistant", 3)]
        [DisplayName("Nickname of the assistant")]
        [Description("Name displayed before assistant's messages in the chat.")]
        public string Nickname { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("AI assistant", 4)]
        [DisplayName("Greeting message")]
        [Description("You can customize the bot's initial words by adding a phrase that triggers conversation on a specific topic.")]
        [DataType(customDataType: KnownFieldTypes.TextArea)]
        public string GreetingMessage { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("AI assistant", 5)]
        [DisplayName("Avatar of the assistant")]
        [Content(Type = "Telerik.Sitefinity.Libraries.Model.Image", AllowMultipleItemsSelection = false, LiveData = false)]
        public MixedContentContext AssistantAvatar { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("AI assistant", 6)]
        [DisplayName("Display sources")]
        [Description("In answers, display links to sources of information.")]
        [DefaultValue(true)]
        [DataType(customDataType: KnownFieldTypes.ChipChoice)]
        [Choice("[{\"Title\":\"Yes\",\"Name\":\"Yes\",\"Value\":\"True\",\"Icon\":null},{\"Title\":\"No\",\"Name\":\"No\",\"Value\":\"False\",\"Icon\":null}]")]
        public bool? ShowSources { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("AI assistant", 7)]
        [DisplayName("Enable visitor feedback")]
        [Description("If enabled, site visitors can provide feedback on the assistant's answer in the chat window.")]
        [DefaultValue(true)]
        [DataType(customDataType: KnownFieldTypes.ChipChoice)]
        [Choice("[{\"Title\":\"Yes\",\"Name\":\"Yes\",\"Value\":\"True\",\"Icon\":null},{\"Title\":\"No\",\"Name\":\"No\",\"Value\":\"False\",\"Icon\":null}]")]
        public bool? ShowFeedback { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chat window", 1)]
        [DisplayName("Chat window mode")]
        [DefaultValue("modal")]
        [DataType(customDataType: KnownFieldTypes.RadioChoice)]
        [Choice("[{\"Title\":\"Display overlay\",\"Name\":\"modal\",\"Value\":\"modal\",\"Icon\":null},{\"Title\":\"Display inline\",\"Name\":\"inline\",\"Value\":\"inline\",\"Icon\":null}]")]
        [Description("[{\"Type\":1,\"Chunks\":[{\"Value\":\"Display overlay: \",\"Presentation\":[0]},{\"Value\":\"Chat appears in a small window, usually in the bottom right corner of the screen. It requires user interaction to open and overlays parts of the page content.\",\"Presentation\":[]}]},{\"Type\":1,\"Chunks\":[{\"Value\":\"Display inline: \",\"Presentation\":[0]},{\"Value\":\"Chat area is integrated into the page layout and does not overlay other elements. Suitable for long assistant responses and prompts.\",\"Presentation\":[]}]}]")]
        public string DisplayMode { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chat window", 2)]
        [DisplayName("Opening chat icon")]
        [Description("Select a custom icon for opening chat window. If left empty, default icon will be displayed.")]
        [Content(Type = "Telerik.Sitefinity.Libraries.Model.Image", AllowMultipleItemsSelection = false)]
        [ConditionalVisibility("{\"conditions\":[{\"fieldName\":\"DisplayMode\",\"operator\":\"Equals\",\"value\":\"modal\"}]}")]
        public MixedContentContext OpeningChatIcon { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chat window", 3)]
        [DisplayName("Closing chat icon")]
        [Description("Select a custom icon for closing chat window. If left empty, default icon will be displayed.")]
        [Content(Type = "Telerik.Sitefinity.Libraries.Model.Image", AllowMultipleItemsSelection = false)]
        [ConditionalVisibility("{\"conditions\":[{\"fieldName\":\"DisplayMode\",\"operator\":\"Equals\",\"value\":\"modal\"}]}")]
        public MixedContentContext ClosingChatIcon { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Chat window", 4)]
        [DisplayName("Container ID")]
        [Description("ID of the HTML element that will host the chat widget.")]
        [DefaultValue("sf-assistant-chat-container")]
        [ConditionalVisibility("{\"conditions\":[{\"fieldName\":\"DisplayMode\",\"operator\":\"Equals\",\"value\":\"inline\"}]}")]
        public string ContainerId { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Message box", 1)]
        [DisplayName("Placeholder text in the message box")]
        [DefaultValue("Ask anything...")]
        public string Placeholder { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection("Message box", 2)]
        [DisplayName("Notice")]
        [Description("Text displayed under the message box, informing users that they are interacting with AI.")]
        [DefaultValue("You are interacting with an AI-powered assistant and the responses are generated by AI.")]
        [DataType(customDataType: KnownFieldTypes.TextArea)]
        public string Notice { get; set; }

        [Category(PropertyCategory.Advanced)]
        [DisplayName("CSS class")]
        public string CssClass { get; set; }

        [Category(PropertyCategory.Advanced)]
        [DisplayName("CSS for custom design")]
        [Placeholder("type URL or path to file...")]
        public string CustomCss { get; set; }

        [Browsable(false)]
        public string EmptyLinkText
        {
            get
            {
                return "Select an AI assistant";
            }
        }

        /// <inheritdoc />
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return KnowledgeBoxName.IsNullOrEmpty();
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
            var viewModel = new PARAGAssistantViewModel(
                this.KnowledgeBoxName,
                this.ConfigurationName,
                this.ShowFeedback.HasValue ? this.ShowFeedback.Value : true,
                this.ShowSources.HasValue ? this.ShowSources.Value : true,
                this.Nickname,
                this.GreetingMessage,
                this.AssistantAvatar,
                !string.IsNullOrEmpty(this.DisplayMode) ? this.DisplayMode : "modal",
                this.OpeningChatIcon,
                this.ClosingChatIcon,
                this.ContainerId,
                this.Placeholder,
                this.Notice,
                this.CustomCss,
                this.CssClass,
                "/parag/",
                "ProgressARAGChatService");

            return View("Index", viewModel);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
        }

        private VersionInfoDto RetrieveVersionInfo(string adminAPIBaseUrl)
        {
            var pathUrl = "/Version";
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri(new Uri(adminAPIBaseUrl),
                pathUrl)
            );

            var httpResponseMessage = this.httpClient.SendAsync(request).GetAwaiter().GetResult();
            httpResponseMessage.EnsureSuccessStatusCode();

            var result = httpResponseMessage.Content.ReadAsAsync<VersionInfoDto>().GetAwaiter().GetResult();

            return result;
        }
    }
}
