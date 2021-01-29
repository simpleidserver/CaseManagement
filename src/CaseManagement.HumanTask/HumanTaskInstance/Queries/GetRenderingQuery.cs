using MediatR;
using Newtonsoft.Json.Linq;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries
{
    public class GetRenderingQuery : IRequest<JObject>
    {
        public string HumanTaskInstanceId { get; set; }
    }
}
