using PARAGCore.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PARAGCore.Client
{
    public interface IPARAGAssistantClient
    {
        Task<HttpResponseMessage> AskAsync(AskRequestDto request);

        Task<string> SendFeedbackAsync(FeedbackRequestDto request);
    }
}