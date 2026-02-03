using PARAGCore.Controllers;
using PARAGCore.OperationProviders;
using System.Net.Http;
using System.Threading.Tasks;

namespace PARAGCore.Client
{
    public interface IPARAGAssistantClient
    {
        /// <summary>
        /// Performs an ask operation in the specified Knowledge Box using the provided query model.
        /// </summary>
        /// <param name="request">The request object containing data for calling the rag endpoint.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the search response.</returns>
        Task<HttpResponseMessage> AskAsync(AskRequestDto request);

        /// <summary>
        /// Sends feedback for a previous ask operation to the PARAG API.
        /// </summary>
        /// <param name="request">The request object containing the feedback data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response as a string.</returns>
        Task<string> SendFeedbackAsync(FeedbackRequestDto request);

        /// <summary>
        /// Retrieves a list of suggestions based on a search query for the specified Knowledge Box.
        /// </summary>
        /// <param name="knowledgeBoxName">The name of the Knowledge Box.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary of search configuration settings.</returns>
        Task<SuggestionsDto> GetSuggestionsAsync(string knowledgeBoxName, string searchQuery);
    }
}