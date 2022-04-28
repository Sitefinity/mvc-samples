namespace NativeChatWidget.Client.DTO
{
    public class NativeChatChannelDTO
    {
        public string Id { get; set; }

        public string ProviderName { get; set; }

        public NativeChatChannelConfig Config { get; set; }
    }
}
