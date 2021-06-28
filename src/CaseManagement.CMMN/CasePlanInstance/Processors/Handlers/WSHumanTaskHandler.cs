using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.Common.Expression;
using CaseManagement.Common.Factories;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors.Handlers
{
    public class WSHumanTaskHandler : IHumanTaskHandler
    {
        private readonly CMMNServerOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public WSHumanTaskHandler(IOptions<CMMNServerOptions> options, CaseManagement.Common.Factories.IHttpClientFactory httpClientFactory)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public string Implementation => CMMNConstants.UserTaskImplementations.WSHUMANTASK;

        public async Task<string> Create(CMMNExecutionContext executionContext, CaseEltInstance humanTaskElt, CancellationToken token)
        {
            using (var httpClient = _httpClientFactory.Build())
            {
                var operationParameters = new JObject
                {
                    { "nameIdentifier", executionContext.Instance.NameIdentifier }
                };

                var inputParameters = humanTaskElt.GetInputParameters();
                if (inputParameters != null && inputParameters.Any())
                {
                    foreach (var inputParameter in inputParameters)
                    {
                        if (string.IsNullOrWhiteSpace(inputParameter.Value))
                        {
                            continue;
                        }

                        var value = ExpressionParser.GetString(inputParameter.Value, executionContext.Instance.ExecutionContext);
                        operationParameters.Add(inputParameter.Key, value);
                    }
                }

                var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = _options.OAuthTokenEndpoint,
                    ClientId = _options.ClientId,
                    ClientSecret = _options.ClientSecret,
                    Scope = "create_humantaskinstance"
                }, token);
                if (tokenResponse.IsError)
                {
                    throw new CMMNProcessorException(tokenResponse.Error);
                }

                var jArr = new JArray();
                var link = _options.CallbackUrl.Replace("{id}", executionContext.Instance.AggregateId);
                link = link.Replace("{eltId}", humanTaskElt.Id);
                jArr.Add(link);
                var obj = new JObject
                {
                    { "humanTaskName", humanTaskElt.GetFormId() },
                    { "operationParameters", operationParameters },
                    { "callbackUrls", jArr }
                };
                var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = content,
                    RequestUri = new Uri($"{_options.WSHumanTaskAPI}/humantaskinstances")
                };
                request.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");
                var httpResult = await httpClient.SendAsync(request, token);
                var str = await httpResult.Content.ReadAsStringAsync();
                var o = JObject.Parse(str);
                var humanTaskInstancId = o["id"].ToString();
                return humanTaskInstancId;
            }
        }
    }
}
