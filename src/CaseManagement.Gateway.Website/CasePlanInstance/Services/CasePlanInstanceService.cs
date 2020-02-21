using CaseManagement.Gateway.Website.CasePlanInstance.DTOs;
using CaseManagement.Gateway.Website.Token;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.Services
{
    public class CasePlanInstanceService : ICasePlanInstanceService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore _tokenStore;
        private readonly ServerOptions _serverOptions;

        public CasePlanInstanceService(HttpClient httpClient, ITokenStore tokenStore, IOptions<ServerOptions> serverOptions)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
            _serverOptions = serverOptions.Value;
        }

        public async Task<FindResponse<CasePlanInstanceResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries)
        {
            var lst = new List<string>();
            foreach (var kvp in queries)
            {
                lst.Add($"{kvp.Key}={kvp.Value}");
            }

            var queryStr = string.Join("&", lst);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/search?{queryStr}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "search_caseplaninstance" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FindResponse<CasePlanInstanceResponse>>(responseStr);
        }

        public async Task<CasePlanInstanceResponse> GetMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me/{id}")
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CasePlanInstanceResponse>(responseStr);
        }        

        public async Task<CasePlanInstanceResponse> AddMe(AddCasePlanInstanceParameter addCasePlanInstanceParameter, string identityToken)
        {
            var json = JsonConvert.SerializeObject(addCasePlanInstanceParameter);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me"),
                Method = HttpMethod.Post,
                Content = new StringContent(json.ToString(), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CasePlanInstanceResponse>(responseStr);
        }

        public async Task LaunchMe(string casePlanInstanceId, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me/{casePlanInstanceId}/launch"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            await _httpClient.SendAsync(request);
        }

        public async Task SuspendMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me/{id}/suspend"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            await _httpClient.SendAsync(request);
        }

        public async Task ReactivateMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me/{id}/reactivate"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            await _httpClient.SendAsync(request);
        }

        public async Task ResumeMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me/{id}/resume"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            await _httpClient.SendAsync(request);
        }

        public async Task TerminateMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me/{id}/terminate"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            await _httpClient.SendAsync(request);
        }

        public async Task CloseMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me/{id}/close"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            await _httpClient.SendAsync(request);
        }

        public async Task<FindResponse<CasePlanInstanceResponse>> SearchMe(IEnumerable<KeyValuePair<string, string>> queries, string identityToken)
        {
            var lst = new List<string>();
            foreach (var kvp in queries)
            {
                lst.Add($"{kvp.Key}={kvp.Value}");
            }

            var queryStr = string.Join("&", lst);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plan-instances/me/search?{queryStr}")
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FindResponse<CasePlanInstanceResponse>>(responseStr);
        }
    }
}
