using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Telerik.Sitefinity.Frontend.Services.PagesService.DTO;
using Telerik.Sitefinity.Web;

namespace PrecompiledViewsCrawler.Crawlers
{
    public class DefaultCrawler : ICrawler
    {
        public void Start()
        {
            string baseUri = UrlPath.ResolveAbsoluteUrl("~/RestApi/pages-api");

            using (HttpWebResponse response = this.MakeWebRequest(baseUri))
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        PagesViewModel responseModel = JsonConvert.DeserializeObject<PagesViewModel>(reader.ReadToEnd());
                        var pageUrls = responseModel.PageUrls;
                        foreach (var item in pageUrls)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                this.MakeWebRequest(item, isCrawled: true);
                            }
                        }
                    }
                }
            }
        }

        private HttpWebRequest CreateStandardWebRequest(string pageUrl)
        {
            var webRequest = WebRequest.Create(pageUrl) as HttpWebRequest;
            if (webRequest == null)
            {
                return null;
            }

            webRequest.Timeout = 120 * 1000; // 120 sec
            webRequest.CookieContainer = new CookieContainer();
            webRequest.Headers.Add("Cookie", HttpContext.Current.Request.Headers["Cookie"]);

            return webRequest;
        }

        private HttpWebResponse MakeWebRequest(string pageUrl, bool isCrawled = false)
        {
            var webRequest = this.CreateStandardWebRequest(pageUrl);
            if (isCrawled)
            {
                webRequest.Headers.Add("X-Crawler", "Enabled");
            }

            var webResponse = webRequest.GetResponse() as HttpWebResponse;
            return webResponse;
        }
    }
}
