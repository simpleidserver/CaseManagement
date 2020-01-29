using CaseManagement.CMMN.AspNetCore.Extensions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.AspNetCore.Apis
{
    [Route(CMMNConstants.RouteNames.CaseDefinitions)]
    public class CaseDefinitionsController : Controller
    {
        private readonly ICaseDefinitionQueryRepository _queryRepository;

        public CaseDefinitionsController(ICaseDefinitionQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        [HttpGet("count")]
        [Authorize("get_statistic")]
        public async Task<IActionResult> Count()
        {
            var result = await _queryRepository.Count();
            return new OkObjectResult(new 
            {
                count = result
            });
        }

        [HttpGet("{id}")]
        [Authorize("get_casedefinition")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/history")]
        [Authorize("get_casedefinition")]
        public async Task<IActionResult> GetHistory(string id)
        {
            var result = await _queryRepository.FindHistoryById(id);
            if (result == null)
            {
                result = new CaseDefinitionHistoryAggregate
                {
                    CaseDefinitionId = id,
                    NbInstances = 0
                };
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("search")]
        [Authorize("get_casedefinition")]
        public async Task<IActionResult> Search()
        {
            var query = HttpContext.Request.Query.ToEnumerable();
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return new OkObjectResult(ToDto(result));
        }

        private static JObject ToDto(FindResponse<CaseDefinition> resp)
        {
            return new JObject
            {
                { "start_index", resp.StartIndex },
                { "total_length", resp.TotalLength },
                { "count", resp.Count },
                { "content", new JArray(resp.Content.Select(r => new JObject
                {
                    { "id", r.Id },
                    { "name", r.Name },
                    { "description", r.Description },
                    { "case_file", r.CaseFileId },
                    { "create_datetime", r.CreateDateTime }
                })) }
            };
        }

        private static JObject ToDto(CaseDefinition def)
        {
            return new JObject
            {
                { "id", def.Id },
                { "name", def.Name },
                { "description", def.Description },
                { "case_file", def.CaseFileId },
                { "create_datetime", def.CreateDateTime }
            };
        }

        private static JObject ToDto(CaseDefinitionHistoryAggregate def)
        {
            return new JObject
            {
                { "id", def.CaseDefinitionId },
                { "nb_instances", def.NbInstances },
                { "elements", new JArray(def.Statistics.Select(s =>
                    new JObject
                    {
                        { "nb_instances", s.NbInstances },
                        { "element", s.CaseElementDefinitionId }
                    }
                ))}
            };
        }

        private static FindWorkflowDefinitionsParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string caseFile;
            string text;
            var parameter = new FindWorkflowDefinitionsParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("case_file", out caseFile))
            {
                parameter.CaseFileId = caseFile;
            }

            if (query.TryGet("text", out text))
            {
                parameter.Text = text;
            }

            return parameter;
        }
    }
}
