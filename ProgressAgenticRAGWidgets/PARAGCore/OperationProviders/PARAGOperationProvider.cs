using PARAGCore.Client;
using PARAGCore.Configuration;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data.Linq.Dynamic;
using Telerik.Sitefinity.Web.Services.Contracts.Operations;

namespace PARAGCore.OperationProviders
{
    public class PARAGOperationProvider : IOperationProvider
    {
        public IEnumerable<OperationData> GetOperations(Type clrType)
        {
            if (clrType == null)
            {
                var getConfiguredKnowledgeBoxesOperation = OperationData.Create<IList<KnowledgeBoxDto>>(this.GetConfiguredKnowledgeBoxes);
                getConfiguredKnowledgeBoxesOperation.OperationType = OperationType.Unbound;

                var assistantVersionOperation = OperationData.Create(this.GetPARAGAssistantVersionInfo);
                assistantVersionOperation.OperationType = OperationType.Unbound;
                assistantVersionOperation.IsAllowedUnauthorized = true;

                var getPARAGSuggestions = OperationData.Create(this.GetPARAGSuggestions);
                getPARAGSuggestions.OperationType = OperationType.Unbound;
                getPARAGSuggestions.IsAllowedUnauthorized = true;

                return new[] { getConfiguredKnowledgeBoxesOperation, assistantVersionOperation, getPARAGSuggestions };
            }

            return Enumerable.Empty<OperationData>();
        }

        private IList<KnowledgeBoxDto> GetConfiguredKnowledgeBoxes(OperationContext context)
        {
            IPARAGAssistantClient client = Telerik.Sitefinity.Abstractions.ObjectFactory.Resolve<IPARAGAssistantClient>();

            var config = Config.Get<AgenticRAGConfig>();
            return config.KnowledgeBoxes
                .AsQueryable<KeyValuePair<string, KnowledgeBoxSettings>>()
                .ToList()
                .Select(x => new KnowledgeBoxDto() 
                { 
                    Name = x.Value.KnowledgeBoxName != null ? x.Value.KnowledgeBoxName : x.Key, 
                    Value = x.Value.KnowledgeBoxName != null ? x.Value.KnowledgeBoxName : x.Key
                })
                .ToList();
        }

        private VersionInfoDto CallVersionInfoEndpoint(string adminAPIBaseUrl)
        {
            if (string.IsNullOrEmpty(adminAPIBaseUrl))
            {
                return null;
            }

            var pathUrl = "Version";
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                new Uri(new Uri(adminAPIBaseUrl),
                pathUrl)
            );

            var httpResponseMessage = new System.Net.Http.HttpClient().SendAsync(request).GetAwaiter().GetResult();
            httpResponseMessage.EnsureSuccessStatusCode();

            var result = httpResponseMessage.Content.ReadAsAsync<VersionInfoDto>().GetAwaiter().GetResult();

            return result;
        }

        private VersionInfoDto GetPARAGAssistantVersionInfo(OperationContext context = null)
        {
            try
            {
                var config = Config.Get<AgenticRAGConfig>().AssistantConfig;
                var adminAPIBaseUrl = config.AdminApiBaseUrl;
                return this.CallVersionInfoEndpoint(adminAPIBaseUrl);
            }
            catch (Exception err)
            {
                Exceptions.HandleException(err, ExceptionPolicyName.IgnoreExceptions);
                return null;
            }
        }

        private IList<string> GetPARAGSuggestions(OperationContext context = null)
        {
            var queryParams = context.GetQueryParams();
            queryParams.TryGetValue("knowledgeBoxName", out var knowledgeBoxName);
            queryParams.TryGetValue("searchQuery", out var searchText);

            try
            {
                var client = ObjectFactory.Resolve<IPARAGAssistantClient>();
                var result = client.GetSuggestionsAsync(knowledgeBoxName, searchText).ConfigureAwait(false).GetAwaiter().GetResult();
                var suggestions = result.Entities.Entities.Select(x => x.Value).Concat(result.Paragraphs.Results.Select(x => x.Text));
                return suggestions.ToList();
            }
            catch (Exception err)
            {
                Exceptions.HandleException(err, ExceptionPolicyName.IgnoreExceptions);
                return null;
            }
        }
    }
}
