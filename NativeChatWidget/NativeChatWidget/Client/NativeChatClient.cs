using NativeChatWidget.Client.DTO;
using NativeChatWidget.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Sitefinity.Configuration;

namespace NativeChatWidget.Client
{
    public class NativeChatClient : INativeChatClient
    {
        private string NativeChatApiEndpoint { get; set; }

        private string ApiKey { get; set; }

        private HttpClient HttpClient { get; set; }

        public NativeChatClient()
        {
            var config = Config.Get<NativeChatConfig>();
            this.ApiKey = config.ApiKey;
            this.NativeChatApiEndpoint = config.NativeChatApiEndpoint;
            this.SetupHttpClient();
        }

        public void Initialize(string apiKey)
        {
            this.ApiKey = apiKey;
            this.NativeChatApiEndpoint = Config.Get<NativeChatConfig>().NativeChatApiEndpoint;
            this.SetupHttpClient();
        }

        private void SetupHttpClient()
        {
            this.HttpClient = new HttpClient();
            this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("api-token", this.ApiKey);
            this.HttpClient.BaseAddress = new Uri(this.NativeChatApiEndpoint);
        }

        public bool HealthCheck()
        {
            HttpResponseMessage response = this.HttpClient.GetAsync("bots").Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        public List<NativeChatBotDTO> Bots()
        {
            var bots = new List<NativeChatBotDTO>();
            HttpResponseMessage response = this.HttpClient.GetAsync("bots").Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                bots = JsonConvert.DeserializeObject<List<NativeChatBotDTO>>(result);
            }

            return bots;
        }


        public NativeChatBotDTO Bot(string botId)
        {
            NativeChatBotDTO bot = null;
            HttpResponseMessage response = this.HttpClient.GetAsync($"bots/{botId}").Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                bot = JsonConvert.DeserializeObject<NativeChatBotDTO>(result);
            }

            return bot;
        }

        public List<NativeChatChannelDTO> BotChannels(string botId)
        {
            var channels = new List<NativeChatChannelDTO>();
            if (!string.IsNullOrEmpty(botId))
            {
                this.ValidateBotId(botId);
                HttpResponseMessage response = this.HttpClient.GetAsync($"bots/{botId}/channels").Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    channels = JsonConvert.DeserializeObject<List<NativeChatChannelDTO>>(result);
                }
            }

            return channels;
        }

        public List<CategoryDTO> Categories(string botId)
        {
            var categories = new List<CategoryDTO>();
            if (!string.IsNullOrEmpty(botId))
            {
                this.ValidateBotId(botId);
                var response = this.HttpClient.GetAsync($"bots/{botId}/entities").Result;

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(result);
            }

            return categories;
        }

        public CategoryDTO CreateCategory(string botId, CategoryDTO category)
        {
            CategoryDTO result = null;
            if (!string.IsNullOrEmpty(botId))
            {
                this.ValidateBotId(botId);

                var stringContent = JsonConvert.SerializeObject(category);
                var content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = this.HttpClient.PostAsync($"bots/{botId}/entities", content).Result;

                response.EnsureSuccessStatusCode();

                var responseString = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<CategoryDTO>(responseString);
            }

            return result;
        }

        public List<GroupDTO> Groups(string botId, string category)
        {
            var groups = new List<GroupDTO>();
            if (!string.IsNullOrEmpty(botId))
            {
                this.ValidateBotId(botId);
                var response = this.HttpClient.GetAsync($"bots/{botId}/entities/{category}/values").Result;

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                groups = JsonConvert.DeserializeObject<List<GroupDTO>>(result);
            }

            return groups;
        }

        public GroupDTO CreateGroup(string botId, string category, GroupDTO group)
        {
            GroupDTO result = null;
            if (!string.IsNullOrEmpty(botId))
            {
                this.ValidateBotId(botId);

                var stringContent = JsonConvert.SerializeObject(group);
                var content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = this.HttpClient.PostAsync($"bots/{botId}/entities/{category}/values", content).Result;

                response.EnsureSuccessStatusCode();

                var responseString = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<GroupDTO>(responseString);
            }

            return result;
        }

        public GroupDTO UpdateGroup(string botId, string category, string groupName, GroupDTO group)
        {
            GroupDTO result = null;
            if (!string.IsNullOrEmpty(botId))
            {
                this.ValidateBotId(botId);

                var stringContent = JsonConvert.SerializeObject(group);
                var content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"bots/{botId}/entities/{category}/values/{groupName}");
                request.Content = content;

                var response = this.HttpClient.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();

                var responseString = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<GroupDTO>(responseString);
            }

            return result;
        }

        public void DeleteGroup(string botId, string category, string groupName)
        {
            if (!string.IsNullOrEmpty(botId))
            {
                this.ValidateBotId(botId);
                var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"bots/{botId}/entities/{category}/values/{groupName}");

                var response = this.HttpClient.SendAsync(request).Result;

                if (response.StatusCode != HttpStatusCode.NotFound) // check if item is already deleted
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        private void ValidateBotId(string botId)
        {
            if (Regex.IsMatch(botId, @"^[a-z0-9]{24}$"))
            {
                return;
            }

            throw new ArgumentException("Invalid bot Id");
        }

        public void Dispose()
        {
            if (this.HttpClient != null)
            {
                this.HttpClient.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
