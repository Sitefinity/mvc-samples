using Newtonsoft.Json;
using PARAGAssistantWidget.Client;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Telerik.Sitefinity.Abstractions;

namespace PARAGAssistantWidget.Controllers
{
    public class AgenticRagController : ApiController
    {
        private readonly IPARAGAssistantClient client;

        public AgenticRagController()
        {
            this.client = ObjectFactory.Resolve<IPARAGAssistantClient>();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Feedback(FeedbackRequestDto model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var response = await this.client.SendFeedbackAsync(model).ConfigureAwait(false);
                return this.Ok(response);
            }
            catch (Exception e)
            {
                Log.Write($"Error while sending feedback: {e.Message}", System.Diagnostics.TraceEventType.Error);
                return this.InternalServerError();
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Chat(AskRequestDto request)
        {
            if (!this.ModelState.IsValid)
            {
                var errorResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var errors = this.ModelState.Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                errorResponse.Content = new StringContent(
                    JsonConvert.SerializeObject(errors),
                    Encoding.UTF8,
                    "application/json"
                );
                return errorResponse;
            }

            var response = await this.client.AskAsync(request).ConfigureAwait(false);
            return response;
        }
    }
}
