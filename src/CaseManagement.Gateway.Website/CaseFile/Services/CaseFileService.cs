using CaseManagement.Gateway.Website.CaseFile.DTOs;
using CaseManagement.Gateway.Website.Token;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CaseFile.Services
{
    public class CaseFileService : ICaseFileService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore _tokenStore;
        private readonly ServerOptions _serverOptions;

        public CaseFileService(HttpClient httpClient, ITokenStore tokenStore, IOptions<ServerOptions> serverOptions)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
            _serverOptions = serverOptions.Value;
        }

        public async Task<string> AddMe(AddCaseFileParameter command, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files/me"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(command).ToString(), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JObject>(responseStr)["id"].ToString();
        }

        public async Task<FindResponse<CaseFileResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries)
        {
            var lst = new List<string>();
            foreach (var kvp in queries)
            {
                lst.Add($"{kvp.Key}={kvp.Value}");
            }

            var queryStr = string.Join("&", lst);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files/search?{queryStr}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_casefile" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FindResponse<CaseFileResponse>>(responseStr);
        }

        public async Task<CaseFileResponse> GetMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files/me/{id}")
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CaseFileResponse>(responseStr);
        }

        public async Task UpdateMe(string id, UpdateCaseFileParameter command, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files/me/{id}"),
                Method = HttpMethod.Put,
                Content = new StringContent(JsonConvert.SerializeObject(command).ToString(), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            await _httpClient.SendAsync(request);
        }

        public async Task<string> PublishMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files/me/{id}/publish"),
                Method = HttpMethod.Get
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JObject.Parse(responseStr)["id"].ToString();
        }
    }
}
