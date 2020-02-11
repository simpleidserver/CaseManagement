using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Token
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ServerOptions _serverOptions;

        public TokenService(HttpClient httpClient, IOptions<ServerOptions> serverOptions)
        {
            _httpClient = httpClient;
            _serverOptions = serverOptions.Value;
        }

        public async Task<string> GetAccessToken(IEnumerable<string> scopes)
        {
            var content = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id",  _serverOptions.ClientId),
                new KeyValuePair<string, string>("client_secret",  _serverOptions.ClientSecret ),
                new KeyValuePair<string, string>("scope", string.Join(" ", scopes)),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_serverOptions.TokenUrl),
                Content = new FormUrlEncodedContent(content)
            };
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            var accessToken = JsonConvert.DeserializeObject<JObject>(responseStr)["access_token"].ToString();
            return accessToken;
        }
    }
}
