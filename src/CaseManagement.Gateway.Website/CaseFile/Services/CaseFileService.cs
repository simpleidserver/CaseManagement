using CaseManagement.Gateway.Website.CaseFile.Commands;
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

        public async Task<string> Add(AddCaseFileCommand command)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(command).ToString(), Encoding.UTF8, "application/json")
            };
            var token = await _tokenStore.GetValidToken(new[] { "add_casefile" });
            request.Headers.Add("Authorization", $"Bearer {token}");
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

        public async Task<CaseFileResponse> Get(string caseFileId)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files/{caseFileId}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_casefile" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CaseFileResponse>(responseStr);
        }

        public async Task Update(UpdateCaseFileCommand command)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files/{command.CaseFileId}"),
                Method = HttpMethod.Put,
                Content = new StringContent(JsonConvert.SerializeObject(command).ToString(), Encoding.UTF8, "application/json")
            };
            var token = await _tokenStore.GetValidToken(new[] { "update_casefile" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            await _httpClient.SendAsync(request);
        }

        public async Task<string> Publish(PublishCaseFileCommand command)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-files/{command.CaseFileId}/publish"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(command).ToString(), Encoding.UTF8, "application/json")
            };
            var token = await _tokenStore.GetValidToken(new[] { "publish_casefile" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JObject.Parse(responseStr)["id"].ToString();
        }
    }
}
