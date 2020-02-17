using CaseManagement.Gateway.Website.CasePlans.DTOs;
using CaseManagement.Gateway.Website.Token;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlans.Services
{
    public class CasePlanService : ICasePlanService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore _tokenStore;
        private readonly ServerOptions _serverOptions;

        public CasePlanService(HttpClient httpClient, ITokenStore tokenStore, IOptions<ServerOptions> serverOptions)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
            _serverOptions = serverOptions.Value;
        }

        public async Task<FindResponse<CasePlanResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries)
        {
            var lst = new List<string>();
            foreach (var kvp in queries)
            {
                lst.Add($"{kvp.Key}={kvp.Value}");
            }

            var queryStr = string.Join("&", lst);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plans/search?{queryStr}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_caseplan" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FindResponse<CasePlanResponse>>(responseStr);
        }

        public async Task<CasePlanResponse> GetMe(string id, string identityToken)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/case-plans/me/{id}")
            };
            request.Headers.Add("Authorization", $"Bearer {identityToken}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CasePlanResponse>(responseStr);
        }
    }
}
