using PARAGCore.Controllers;
using PARAGResultsWidget.Mvc.Models;
using Progress.Sitefinity.Renderer.Designers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web.Mvc;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Web.Services.Contracts.Operations.Pages.PropertyEditor.AttributeConfigurator.Attributes;

namespace PARAGResultsWidget.Mvc.Controllers
{
    [Telerik.Sitefinity.Mvc.ControllerToolboxItem(
        Name = WidgetName,
        Title = "PARAG results",
        SectionName = SectionName,
        Ordinal = 3,
        CssClass = WidgetIconCssClass)]
    public class PARAGResultsController : Controller
    {
        internal const string WidgetName = "PARAGResults_MVC";
        internal const string SectionName = "AI search";
        private const string WidgetIconCssClass = "sfSearchResultIcn sfMvcIcn";
        private const string ResultsListSettings = "AI results list settings";
        private const string DisplaySettingsSectionName = "Display settings";
        private const string LabelsSectionName = "Labels and messages";
        private readonly HttpClient httpClient;

        public PARAGResultsController()
        {
            this.httpClient = new HttpClient();
        }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection(ResultsListSettings, 0)]
        [DisplayName("Results per page")]
        [DefaultValue(20)]
        [Range(1, 200)]
        public int? PageSize { get; set; }

        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection(DisplaySettingsSectionName, 0)]
        [ViewSelector]
        [DisplayName("AI results template")]
        [DefaultValue("Default")]
        public string SfViewName { get; set; }

        [Category(PropertyCategory.Advanced)]
        [DisplayName("CSS class")]
        public string CssClass { get; set; }

        [Category(PropertyCategory.Advanced)]
        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection(LabelsSectionName, 0)]
        [DisplayName("Search results header")]
        [DefaultValue("Results for \"{0}\"")]
        public string SearchResultsHeader { get; set; }

        [Category(PropertyCategory.Advanced)]
        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection(LabelsSectionName, 1)]
        [DisplayName("No results header")]
        [DefaultValue("No results for \"{0}\"")]
        public string NoResultsHeader { get; set; }

        [Category(PropertyCategory.Advanced)]
        [Progress.Sitefinity.Renderer.Designers.Attributes.ContentSection(LabelsSectionName, 2)]
        [DisplayName("Results number label")]
        [DefaultValue("results")]
        public string ResultsNumberLabel { get; set; }

        public ActionResult Index()
        {
            var searchQuery = HttpContext.Request.QueryStringGet("searchQuery");
            var knowledgeBoxName = HttpContext.Request.QueryStringGet("knowledgeBoxName");

            var model = new ResultsViewModel();
            model.CssClass = this.CssClass;
            model.ResultsHeader = string.Format(CultureInfo.InvariantCulture, this.NoResultsHeader ?? "No results for \"{0}\"", searchQuery);
            model.ResultsNumberLabel = this.ResultsNumberLabel ?? "results";
            model.PageSize = this.PageSize ?? 20;

            if (!string.IsNullOrEmpty(searchQuery) && !string.IsNullOrEmpty(knowledgeBoxName))
            {
                model.SearchResults = new List<SearchResultModel>();
                FindResponseDto response = this.PerformSearch();

                if (response != null && response.Resources != null && response.Resources.Count > 0)
                {
                    foreach (var resourceEntry in response.Resources)
                    {
                        var resource = resourceEntry.Value;

                        if (resource.Origin != null)
                        {
                            var result = new SearchResultModel
                            {
                                Title = resource.Title,
                                Link = resource.Origin.Url,
                            };

                            var allParagraphs = new List<Paragraph>();
                            if (resource.Fields != null)
                            {
                                foreach (var fieldEntry in resource.Fields)
                                {
                                    if (fieldEntry.Value?.Paragraphs != null)
                                    {
                                        foreach (var paraEntry in fieldEntry.Value.Paragraphs)
                                        {
                                            allParagraphs.Add(paraEntry.Value);
                                        }
                                    }
                                }
                            }

                            allParagraphs.Sort((a, b) => a.Order.CompareTo(b.Order));
                            result.Order = allParagraphs.FirstOrDefault()?.Order ?? 0;
                            model.SearchResults.Add(result);
                        }
                    }

                    model.SearchResults.Sort((a, b) => a.Order.CompareTo(b.Order));

                    if (model.SearchResults.Count > 0)
                    {
                        model.ResultsHeader = string.Format(CultureInfo.InvariantCulture, this.SearchResultsHeader ?? "Results for \"{0}\"", searchQuery);
                    }
                }
            }

            return View(this.SfViewName ?? "Default", model);
        }

        private FindResponseDto PerformSearch()
        {
            var searchQuery = HttpContext.Request.QueryStringGet("searchQuery");
            var knowledgeBoxName = HttpContext.Request.QueryStringGet("knowledgeBoxName");

            if (!string.IsNullOrEmpty(knowledgeBoxName) && !string.IsNullOrEmpty(searchQuery))
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    new Uri(RouteHelper.ResolveUrl("/parag/find", UrlResolveOptions.Absolute))
                );

                var searchConfiguarionName = HttpContext.Request.QueryStringGet("searchConfigurationName");

                var findRequest = new FindRequestDto()
                {
                    KnowledgeBoxName = knowledgeBoxName,
                    Query = searchQuery,
                    ConfigurationName = searchConfiguarionName,
                    Take = 200,
                    Show = new string[] { "basic", "origin", "values" }
                };

                request.Content = new StringContent(JsonSerializer.Serialize(findRequest), Encoding.UTF8, "application/json");

                var httpResponseMessage = this.httpClient.SendAsync(request).GetAwaiter().GetResult();
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = httpResponseMessage.Content.ReadAsAsync<FindResponseDto>().GetAwaiter().GetResult();

                return response;
            }

            return null;
        }
    }
}
