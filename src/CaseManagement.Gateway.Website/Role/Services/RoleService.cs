using CaseManagement.Gateway.Website.Role.DTOs;
using CaseManagement.Gateway.Website.Token;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Role.Services
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore _tokenStore;
        private readonly ServerOptions _serverOptions;

        public RoleService(HttpClient httpClient, ITokenStore tokenStore, IOptions<ServerOptions> serverOptions)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
            _serverOptions = serverOptions.Value;
        }

        public async Task<RoleResponse> Add(AddRoleParameter parameter)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/roles"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(parameter).ToString(), Encoding.UTF8, "application/json")
            };
            var token = await _tokenStore.GetValidToken(new[] { "add_role" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            httpResponse.EnsureSuccessStatusCode();
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RoleResponse>(responseStr);
        }

        public async Task Update(string role, UpdateRoleParameter parameter)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/roles/{role}"),
                Method = HttpMethod.Put,
                Content = new StringContent(JsonConvert.SerializeObject(parameter).ToString(), Encoding.UTF8, "application/json")
            };
            var token = await _tokenStore.GetValidToken(new[] { "update_role" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task<RoleResponse> Get(string role)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/roles/{role}"),
                Method = HttpMethod.Get
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_role" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            httpResponse.EnsureSuccessStatusCode();
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RoleResponse>(responseStr);
        }
        
        public async Task Delete(string role)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/roles/{role}"),
                Method = HttpMethod.Delete
            };
            var token = await _tokenStore.GetValidToken(new[] { "delete_role" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task<FindResponse<RoleResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries)
        {
            var lst = new List<string>();
            foreach (var kvp in queries)
            {
                lst.Add($"{kvp.Key}={kvp.Value}");
            }

            var queryStr = string.Join("&", lst);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/roles/search?{queryStr}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "search_role" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<FindResponse<RoleResponse>>(responseStr);
        }
    }
}
