using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.Common.Expression;
using CaseManagement.Common.Factories;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
                        { "nameIdentifier", executionContext.Instance.NameIdentifier }
                    };
                    var ctx = new ConditionalExpressionContext(pointer.Incoming.ToList());
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

                    var jArr = new JArray();
                    var link = _options.CallbackUrl.Replace("{id}", executionContext.Instance.AggregateId);
                    link = link.Replace("{eltId}", pointer.InstanceFlowNodeId);
                    jArr.Add(link);
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
                        { "operationParameters", operationParameters },
                        { "callbackUrls",  jArr }
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
                    var jObj = new JObject();
                    jObj.Add("id", humanTaskInstancId);
                    var incoming = executionContext.Pointer.Incoming.ToList();
                    incoming.Add(new MessageToken
                    {
                        Name = "humanTaskInstance",
                        MessageContent = jObj.ToString()
                    });
                    var messageContent = new JObject();
                    messageContent.Add("incoming", JArray.FromObject(incoming));
                    executionContext.Instance.ConsumeMessage(new MessageToken
                    {
                        Name = "humanTaskCreated",
                        MessageContent = messageContent.ToString()
                    });
                    throw new FlowNodeInstanceRestartedException();
                }
            }

            var stateTransition = executionContext.Instance.StateTransitions.FirstOrDefault(_ => _.StateTransition == "COMPLETED" && _.FlowNodeInstanceId == pointer.InstanceFlowNodeId);
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
                result.Add(MessageToken.NewMessage(userTask.EltId, stateTransition.Content));
            }

            return result;
        }
    }
}
