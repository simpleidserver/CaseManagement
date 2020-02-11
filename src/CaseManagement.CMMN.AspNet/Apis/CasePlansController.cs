using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Extensions;
using CaseManagement.CMMN.Persistence;
using CaseManagement.CMMN.Persistence.Parameters;
using CaseManagement.CMMN.Persistence.Responses;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Apis
{
    [RoutePrefix(CMMNConstants.RouteNames.CasePlans)]
    public class CasePlansController : ApiController
    {
        private readonly ICasePlanQueryRepository _queryRepository;

        public CasePlansController(ICasePlanQueryRepository queryRepository)
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
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return Ok(ToDto(result));
        }

        private static JObject ToDto(FindResponse<CasePlanAggregate> resp)
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

        private static JObject ToDto(CasePlanAggregate def)
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

        private static FindCasePlansParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            string caseFile;
            string text;
            string caseOwner;
            var parameter = new FindCasePlansParameter();
            parameter.ExtractFindParameter(query);
            if (query.TryGet("case_file", out caseFile))
            {
                parameter.CaseFileId = caseFile;
            }

            if (query.TryGet("text", out text))
            {
                parameter.Text = text;
            }

            if (query.TryGet("caseowner", out caseOwner))
            {
                parameter.CaseOwner = caseOwner;
            }

            return parameter;
        }
    }
}
