using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Http;
using System.Net.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{  
    [RoutePrefix(CMMNConstants.RouteNames.CaseDefinitions)]
    public class CaseDefinitionsController : ApiController
    {
        private readonly ICaseDefinitionQueryRepository _queryRepository;

        public CaseDefinitionsController(ICaseDefinitionQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        [HttpGet]
        [Route("count")]
        public async Task<IHttpActionResult> Count()
        {
            var result = await _queryRepository.Count();
            return Ok(new
            {
                count = result
            });
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(ToDto(result));
        }

        [HttpGet]
        [Route("{id}/history")]
        public async Task<IHttpActionResult> GetHistory(string id)
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

            return Ok(ToDto(result));
        }

        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return Ok(ToDto(result));
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
