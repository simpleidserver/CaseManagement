using System.Net.Http;

namespace CaseManagement.Common.Factories
{
    public interface IHttpClientFactory
    {
        HttpClient Build();
    }
}
