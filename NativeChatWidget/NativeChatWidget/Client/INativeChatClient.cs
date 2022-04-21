using NativeChatWidget.Client.DTO;
using System;
using System.Collections.Generic;

namespace NativeChatWidget.Client
{
    public interface INativeChatClient : IDisposable
    {
        void Initialize(string apiKey);

        bool HealthCheck();

        List<NativeChatBotDTO> Bots();

        NativeChatBotDTO Bot(string botId);

        List<NativeChatChannelDTO> BotChannels(string botId);

        List<CategoryDTO> Categories(string botId);

        CategoryDTO CreateCategory(string botId, CategoryDTO category);

        List<GroupDTO> Groups(string botId, string category);

        GroupDTO CreateGroup(string botId, string category, GroupDTO group);

        GroupDTO UpdateGroup(string botId, string category, string groupName, GroupDTO group);

        void DeleteGroup(string botId, string category, string groupName);
    }
}