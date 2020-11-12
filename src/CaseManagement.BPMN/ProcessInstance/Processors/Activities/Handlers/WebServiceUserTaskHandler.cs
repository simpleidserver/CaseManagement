using CaseManagement.BPMN.Common;
using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using CaseManagement.Common.Factories;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.ProcessInstance.Processors.Activities.Handlers
{
    public class WebServiceUserTaskHandler : IUserServerTaskHandler
    {
        private readonly BPMNServerOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string HUMANTASK_INSTANCE_ID_NAME = "humantaskinstanceid";

        public WebServiceUserTaskHandler(IOptions<BPMNServerOptions> options,
            IHttpClientFactory httpClientFactory)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public string Implementation => BPMNConstants.UserTaskImplementations.WEBSERVICE;

        public async Task<ICollection<BaseToken>> Execute(BPMNExecutionContext executionContext, UserTask userTask, CancellationToken token)
        {
            var pointer = executionContext.Pointer;
            var instance = executionContext.Instance.GetInstance(pointer.InstanceFlowNodeId);
            if (!instance.Metadata.ContainsKey(HUMANTASK_INSTANCE_ID_NAME))
            {
                using (var httpClient = _httpClientFactory.Build())
                {
                    var obj = new JObject
                    {
                        { "humanTaskName", userTask.HumanTaskName }
                    };
                    var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        Content = content,
                        RequestUri = new System.Uri($"{_options.WSHumanTaskAPI}/humantaskinstances")
                    };
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

            return new List<BaseToken> { stateTransition };
        }
    }
}
