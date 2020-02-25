using CaseManagement.Gateway.Website.Form.DTOs;
using CaseManagement.Gateway.Website.Token;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Form.Services
{
    public class FormService : IFormService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenStore _tokenStore;
        private readonly ServerOptions _serverOptions;

        public FormService(HttpClient httpClient, ITokenStore tokenStore, IOptions<ServerOptions> serverOptions)
        {
            _httpClient = httpClient;
            _tokenStore = tokenStore;
            _serverOptions = serverOptions.Value;
        }

        public async Task<FormResponse> Get(string id)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/forms/{id}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "get_form" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(responseStr);
            return ToFormResponse(jObj);
        }

        public async Task<FindResponse<FormResponse>> Search(IEnumerable<KeyValuePair<string, string>> queries)
        {
            var lst = new List<string>();
            foreach (var kvp in queries)
            {
                lst.Add($"{kvp.Key}={kvp.Value}");
            }

            var queryStr = string.Join("&", lst);
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{_serverOptions.ApiUrl}/forms/search?{queryStr}")
            };
            var token = await _tokenStore.GetValidToken(new[] { "search_form" });
            request.Headers.Add("Authorization", $"Bearer {token}");
            var httpResponse = await _httpClient.SendAsync(request);
            var responseStr = await httpResponse.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(responseStr);
            return new FindResponse<FormResponse>
            {
                StartIndex = int.Parse(jObj["start_index"].ToString()),
                Count = int.Parse(jObj["count"].ToString()),
                TotalLength = int.Parse(jObj["total_length"].ToString()),
                Content = (jObj["content"] as JArray).Select(o => ToFormResponse(o as JObject)).ToList()
            };
        }
        
        private static FormResponse ToFormResponse(JObject jObj)
        {
            var formResponse = new FormResponse
            {
                Id = jObj["id"].ToString(),
                Version = int.Parse(jObj["version"].ToString()),
                CreateDateTime = DateTime.Parse(jObj["create_datetime"].ToString()),
                UpdateDateTime = DateTime.Parse(jObj["update_datetime"].ToString()),
                Status = jObj["status"].ToString(),
                Elements = new List<FormElementResponse>(),
                Titles = new List<TranslationResponse>()
            };
            foreach (var record in jObj)
            {
                if (record.Key.StartsWith("title"))
                {
                    formResponse.Titles.Add(new TranslationResponse
                    {
                        Language = record.Key.ToString().Split('#').Last(),
                        Value = record.Value.ToString()
                    });
                }
            }

            foreach (JObject record in jObj["elements"] as JArray)
            {
                var elt = new FormElementResponse
                {
                    Id = record["id"].ToString(),
                    IsRequired = bool.Parse(record["is_required"].ToString()),
                    Type = record["type"].ToString(),
                    Tiles = new List<TranslationResponse>(),
                    Descriptions = new List<TranslationResponse>()
                };
                foreach (var r in record)
                {
                    if (r.Key.StartsWith("title"))
                    {
                        elt.Tiles.Add(new TranslationResponse
                        {
                            Language = r.Key.ToString().Split('#').Last(),
                            Value = r.Value.ToString()
                        });
                    }
                    else if (r.Key.StartsWith("description"))
                    {
                        elt.Descriptions.Add(new TranslationResponse
                        {
                            Language = r.Key.ToString().Split('#').Last(),
                            Value = r.Value.ToString()
                        });
                    }
                }

                formResponse.Elements.Add(elt);
            }

            return formResponse;
        }
    }
}
