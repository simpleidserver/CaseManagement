using CaseManagement.Gateway.Website.Statistic.DTOs;
using CaseManagement.Gateway.Website.Token;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Statistic.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore _tokenStore;
        private readonly ServerOptions _serverOptions;

        public StatisticService(HttpClient httpClient, ITokenStore tokenStore, IOptions<ServerOptions> serverOptions)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
            _serverOptions = serverOptions.Value;
        }

        public async Task<DailyStatisticResponse> Get()
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/statistics")
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_statistic" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<DailyStatisticResponse>(responseStr);
        }

        public async Task<FindDailyStatisticResponse> Search(IEnumerable<KeyValuePair<string, string>> queries)
        {
            var lst = new List<string>();
            foreach(var kvp in queries)
            {
                lst.Add($"{kvp.Key}={kvp.Value}");
            }

            var queryStr = string.Join("&", lst);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/statistics?{queryStr}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_statistic" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FindDailyStatisticResponse>(responseStr);
        }
    }
}