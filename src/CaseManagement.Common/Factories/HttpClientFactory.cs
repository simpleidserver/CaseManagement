using System.Net.Http;

namespace CaseManagement.Common.Factories
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient Build()
        {
            return new HttpClient();
        }
    }
}
