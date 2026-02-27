using PARAGCore.Clients.Models.Serialization;
using PARAGCore.Configuration;
using PARAGCore.Controllers;
using PARAGCore.OperationProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telerik.Sitefinity.Configuration;

namespace PARAGCore.Client
{
    public class PARAGAssistantClient : IPARAGAssistantClient
    {
        protected const string NucliaServiceAccountHeader = "X-NUCLIA-SERVICEACCOUNT";

        private static readonly HttpClient sharedHttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(100)
        };

        public PARAGAssistantClient()
        {
            HttpClient = sharedHttpClient;
        }

        /// <inheritdoc/>
        public async Task<HttpResponseMessage> AskAsync(AskRequestDto request)
        {
            var knowledgeBoxName = request.KnowledgeBoxName;

            // remove kbname from payload
            request.KnowledgeBoxName = null;

            return this.SendKBStreamingRequestAsync(
                knowledgeBoxName,
                "ask",
                HttpMethod.Post,
                request).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public async Task<string> SendFeedbackAsync(FeedbackRequestDto request)
        {
            var knowledgeBoxName = request.KnowledgeBoxName;

            // remove kbname from payload
            request.KnowledgeBoxName = null;

            return this.SendKBRequestAsync<string>(
                knowledgeBoxName,
                "feedback",
                HttpMethod.Post,
                request).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public Task<SuggestionsDto> GetSuggestionsAsync(string knowledgeBoxName, string searchQuery)
        {
            return this.SendKBRequestAsync<SuggestionsDto>(
                knowledgeBoxName,
                $"suggest?query={searchQuery}&fields=a/title&features=entities&features=paragraph",
                HttpMethod.Get);
        }

        public async Task<FindResponseDto> FindAsync(FindRequestDto request)
        {
            var knowledgeBoxName = request.KnowledgeBoxName;

            // remove kbname from payload
            request.KnowledgeBoxName = null;
            
            return await SendKBRequestAsync<FindResponseDto>(knowledgeBoxName, "find", HttpMethod.Post, request).ConfigureAwait(false);
        }

        private async Task<Dictionary<string, string>> SetAuthHeaders(Dictionary<string, string> headers, string knowledgeBoxName)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            var config = Config.Get<PARAGConfig>();
            string accessKey = config.KnowledgeBoxes[knowledgeBoxName].KnowledgeBoxToken;
            headers.Add(NucliaServiceAccountHeader, $"Bearer {accessKey}");

            return headers;
        }

        private async Task<T> SendKBRequestAsync<T>(
           string knowledgeBoxName,
           string endpoint,
           HttpMethod method,
           object payload = null,
           Dictionary<string, string> headers = null)
        {
            headers = await SetAuthHeaders(headers, knowledgeBoxName).ConfigureAwait(false);

            var config = Config.Get<PARAGConfig>();

            if (config.KnowledgeBoxes.TryGetValue(knowledgeBoxName, out var kbSettings))
            {
                return await SendRequestAsync<T>(kbSettings.KnowledgeBoxId, $"/api/v1/kb/{kbSettings.KnowledgeBoxId}/{endpoint}", method, payload, headers).ConfigureAwait(false);
            }

            return default(T);
        }

        private async Task<HttpResponseMessage> SendKBStreamingRequestAsync(
           string knowledgeBoxName,
           string endpoint,
           HttpMethod method,
           object payload = null,
           Dictionary<string, string> headers = null)
        {
            headers = await SetAuthHeaders(headers, knowledgeBoxName).ConfigureAwait(false);

            var config = Config.Get<PARAGConfig>();

            if (config.KnowledgeBoxes.TryGetValue(knowledgeBoxName, out var kbSettings))
            {
                return await SendStreamingRequestAsync(kbSettings.KnowledgeBoxId, $"/api/v1/kb/{kbSettings.KnowledgeBoxId}/{endpoint}", method, payload, headers).ConfigureAwait(false);
            }

            return null;
        }

        protected virtual async Task<HttpResponseMessage> SendRawHttpRequest(
           string knowledgeBoxId,
           string endpoint,
           HttpMethod method,
           object payload = null,
           Dictionary<string, string> headers = null,
           HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var config = Config.Get<PARAGConfig>();
            var kb = config.KnowledgeBoxes.Values.Where(x => x.KnowledgeBoxId == knowledgeBoxId).FirstOrDefault();

            if (kb == null)
            {
                return null;
            }

            HttpRequestMessage request = new HttpRequestMessage(method, $"{kb.BaseUrl}{endpoint}");

            // Add custom headers
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            // Add payload for POST/PUT/PATCH requests
            if (payload != null && (method == HttpMethod.Post || method == HttpMethod.Put))
            {
                string json = PARAGJSONSerializer.Serialize(payload);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = await HttpClient.SendAsync(request, completionOption).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new HttpRequestException($"HTTP {(int)response.StatusCode} {response.StatusCode}: {errorContent}");
            }

            return response;
        }

        protected virtual async Task<T> SendRequestAsync<T>(
            string knowledgeBoxId,
            string endpoint,
            HttpMethod method,
            object payload = null,
            Dictionary<string, string> headers = null)
        {
            var response = await SendRawHttpRequest(knowledgeBoxId, endpoint, method, payload, headers, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return default(T);
            }

            try
            {
                var res = PARAGJSONSerializer.Deserialize<T>(responseContent);
                return res;
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Failed to deserialize response: {ex.Message}\nResponse content: {responseContent}", ex);
            }
        }

        protected virtual async Task<HttpResponseMessage> SendStreamingRequestAsync(
            string knowledgeBoxId,
            string endpoint,
            HttpMethod method,
            object payload = null,
            Dictionary<string, string> headers = null)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }

            headers["Accept-Encoding"] = "gzip, deflate";

            var response = await SendRawHttpRequest(knowledgeBoxId, endpoint, method, payload, headers, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            return response;
        }

        protected HttpClient HttpClient { get; private set; }
    }
}
