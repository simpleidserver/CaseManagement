using CaseManagement.BPMN.Common;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.Common.Expression;
using CaseManagement.Common.Factories;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Activities.Handlers
{
    public class WsHumanTaskHandler : IUserServerTaskHandler
    {
        private readonly BPMNServerOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string HUMANTASK_INSTANCE_ID_NAME = "humantaskinstanceid";

        public WsHumanTaskHandler(IOptions<BPMNServerOptions> options,
            IHttpClientFactory httpClientFactory)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public string Implementation => BPMNConstants.UserTaskImplementations.WSHUMANTASK;

        public async Task<ICollection<MessageToken>> Execute(BPMNExecutionContext executionContext, UserTask userTask, CancellationToken token)
        {
            var pointer = executionContext.Pointer;
            var instance = executionContext.Instance.GetInstance(pointer.InstanceFlowNodeId);
            if (!instance.Metadata.ContainsKey(HUMANTASK_INSTANCE_ID_NAME))
            {
                using (var httpClient = _httpClientFactory.Build())
                {
                    var operationParameters = new JObject
                    {
                        { "flowNodeInstanceId", executionContext.Instance.AggregateId },
                        { "flowNodeElementInstanceId", pointer.InstanceFlowNodeId },
                        { "nameIdentifier", executionContext.Instance.NameIdentifier }
                    };
                    var ctx = new ConditionalExpressionContext(pointer.Incoming);
                    if (userTask.InputParameters != null && userTask.InputParameters.Any())
                    {
                        foreach(var inputParameter in userTask.InputParameters)
                        {
                            if (string.IsNullOrWhiteSpace(inputParameter.Value))
                            {
                                continue;
                            }

                            var value = ExpressionParser.GetString(inputParameter.Value, ctx);
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
                        throw new BPMNProcessorException(tokenResponse.Error);
                    }

                    var obj = new JObject
                    {
                        { "humanTaskName", userTask.HumanTaskName },
                        { "operationParameters", operationParameters }
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
                    executionContext.Instance.UpdateMetadata(pointer.InstanceFlowNodeId, HUMANTASK_INSTANCE_ID_NAME, humanTaskInstancId);
                    throw new FlowNodeInstanceBlockedException();
                }
            }

            var stateTransition = executionContext.Instance.StateTransitions.FirstOrDefault(_ => _.State == "COMPLETED" && _.FlowNodeInstanceId == pointer.InstanceFlowNodeId);
            if (stateTransition == null)
            {
                throw new FlowNodeInstanceBlockedException();
            }

            var result = new List<MessageToken>();
            if (stateTransition.Content == null)
            {
                result.Add(MessageToken.EmptyMessage());
            }
            else
            {
                result.Add(MessageToken.NewMessage(userTask.Id, stateTransition.Content));
            }

            return result;
        }
    }
}
