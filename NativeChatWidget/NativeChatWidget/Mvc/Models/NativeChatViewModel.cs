using NativeChatWidget.Client;
using NativeChatWidget.Mvc.Controllers;
using Progress.Sitefinity.Renderer.Entities.Content;
using System;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Modules.Libraries;

namespace NativeChatWidget.Mvc.Models
{
    public class NativeChatViewModel
    {
        public NativeChatViewModel(
            string botId, string nickname,
            MixedContentContext botAvatar,
            string userMessage,
            ChatWindowMode chatMode,
            string placeholder,
            ChatPickers showPickers,
            MixedContentContext openingChatIcon,
            MixedContentContext closingChatIcon,
            string containerId,
            string locationPickerLabel,
            string googleApiKey,
            string customCss,
            string locale,
            string defaultLocation)
        {
            this.BotId = botId;
            this.SetChannel(botId);
            this.Nickname = nickname;
            this.SetImageUrl(botAvatar, "BotAvatarUrl");
            this.UserMessage = userMessage;
            this.ChatMode = chatMode;
            this.Placeholder = placeholder;
            this.SetChatPickers(showPickers);
            this.SetImageUrl(openingChatIcon, "OpeningChatIconUrl");
            this.SetImageUrl(closingChatIcon, "ClosingChatIconUrl");
            this.ContainerId = containerId;
            this.LocationPickerLabel = locationPickerLabel;
            this.GoogleApiKey = googleApiKey;
            this.SetDefaultLocation(defaultLocation);
            this.CustomCss = customCss;
            this.Locale = locale;
        }

        #region Properties

        public string BotId { get; set; }

        public string ChannelId { get; set; }

        public string ChannelAuthToken { get; set; }

        public string Nickname { get; set; }

        public string BotAvatarUrl { get; set; }

        public string UserMessage { get; set; }

        public ChatWindowMode ChatMode { get; set; }

        public string Placeholder { get; set; }

        public string ShowFilePicker { get; set; }

        public string ShowLocationPicker { get; set; }

        public string OpeningChatIconUrl { get; set; }

        public string ClosingChatIconUrl { get; set; }

        public string ContainerId { get; set; }

        public string LocationPickerLabel { get; set; }

        public string GoogleApiKey { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string CustomCss { get; set; }

        public string Locale { get; set; }

        #endregion

        private void SetChannel(string botId)
        {
            var client = ObjectFactory.Resolve<INativeChatClient>();
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            using (client)
            {
                var channels = client.BotChannels(botId);

                // NativeChat specific: web channels have a providerName == "darvin" 
                var webChannel = channels.Find(x => x.ProviderName == "darvin");
                if (webChannel != null)
                {
                    this.ChannelId = webChannel.Id;
                    this.ChannelAuthToken = webChannel.Config.AuthToken;
                }
            }
        }

        private void SetImageUrl(MixedContentContext image, string propName)
        {
            if (image != null)
            {
                var imageProvider = image.Content[0].Variations[0].Source;
                var imageId = new Guid(image.ItemIdsOrdered[0]);
                var librariesManager = LibrariesManager.GetManager(imageProvider);
                var imageUrl = librariesManager.GetMediaItem(imageId).Url;
                var prop = this.GetType().GetProperty(propName);
                prop.SetValue(this, imageUrl);
            }
        }

        private void SetChatPickers(ChatPickers showPickers)
        {
            var showPickersStr = showPickers.ToString();
            ShowFilePicker = showPickersStr.Contains(ChatPickers.FilePicker.ToString()).ToString().ToLower();
            ShowLocationPicker = showPickersStr.Contains(ChatPickers.LocationPicker.ToString()).ToString().ToLower();
        }

        private void SetDefaultLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
                return;

            var coordinates = location.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (coordinates.Length == 2)
            {
                double latitude;
                double longitude;

                if (Double.TryParse(coordinates[0], out latitude) && Double.TryParse(coordinates[1], out longitude))
                {
                    this.Latitude = latitude;
                    this.Longitude = longitude;
                }
            }
        }
    }
}
