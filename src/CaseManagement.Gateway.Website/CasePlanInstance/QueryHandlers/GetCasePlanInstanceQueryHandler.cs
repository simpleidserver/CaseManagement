using CaseManagement.Gateway.Website.CasePlanInstance.Queries;
using CaseManagement.Gateway.Website.CasePlanInstance.Results;
using CaseManagement.Gateway.Website.CasePlanInstance.Services;
using CaseManagement.Gateway.Website.Form.Services;
using CaseManagement.Gateway.Website.FormInstance.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.CasePlanInstance.QueryHandlers
{
    public class GetCasePlanInstanceQueryHandler : IGetCasePlanInstanceQueryHandler
    {
        private readonly ICasePlanInstanceService _casePlanInstanceService;
        private readonly IFormInstanceService _formInstanceService;
        private readonly IFormService _formService;

        public GetCasePlanInstanceQueryHandler(ICasePlanInstanceService casePlanInstanceService, IFormInstanceService formInstanceService, IFormService formService)
        {
            _casePlanInstanceService = casePlanInstanceService;
            _formInstanceService = formInstanceService;
            _formService = formService;
        }

        public async Task<GetCasePlanInstanceResult> Handle(GetCasePlanInstanceQuery query)
        {
            var casePlanInstance = await _casePlanInstanceService.Get(query.CasePlanInstanceId);
            var formInstanceQueries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("case_plan_instance_id", query.CasePlanInstanceId)
            };
            var formInstanceSearchResult = await _formInstanceService.Search(formInstanceQueries);
            var formQueries = new List<KeyValuePair<string, string>>();
            foreach(var formId in formInstanceSearchResult.Content.Select(f => f.FormId))
            {
                formQueries.Add(new KeyValuePair<string, string>("id", formId));
            }

            var forms = await _formService.Search(formQueries);
            return new GetCasePlanInstanceResult
            {
                CasePlanInstance = casePlanInstance,
                FormInstances = formInstanceSearchResult,
                Forms = forms
            };
        }
    }
}
