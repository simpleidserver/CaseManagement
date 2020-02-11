using CaseManagement.Gateway.Website.Performance.DTOs;
using CaseManagement.Gateway.Website.Token;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Performance.Services
{
    public class PerformanceService : IPerformanceService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore _tokenStore;
        private readonly ServerOptions _serverOptions;

        public PerformanceService(HttpClient httpClient, ITokenStore tokenStore, IOptions<ServerOptions> serverOptions)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
            _serverOptions = serverOptions.Value;
        }

        public async Task<IEnumerable<string>> Get()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/performances")
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_performance" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<string>>(responseStr);
        }

        public async Task<FindPerformanceResponse> Search(IEnumerable<KeyValuePair<string, string>> queries)
        {
            var lst = new List<string>();
            foreach (var kvp in queries)
            {
                lst.Add($"{kvp.Key}={kvp.Value}");
            }

            var queryStr = string.Join("&", lst);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/performances/search?{queryStr}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_performance" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FindPerformanceResponse>(responseStr);
        }
    }
}
