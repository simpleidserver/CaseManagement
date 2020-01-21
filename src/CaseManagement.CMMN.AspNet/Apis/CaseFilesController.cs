using CaseManagement.CMMN.Domains.CaseFile;
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
    [RoutePrefix(CMMNConstants.RouteNames.CaseFiles)]
    public class CaseFilesController : ApiController
    {
        private readonly ICaseFileQueryRepository _queryRepository;

        public CaseFilesController(ICaseFileQueryRepository queryRepository)
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
        [Route("search")]
        public async Task<IHttpActionResult> Search()
        {
            var query = this.Request.GetQueryNameValuePairs();
            var result = await _queryRepository.Find(ExtractFindParameter(query));
            return Ok(ToDto(result));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var result = await _queryRepository.FindById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(ToDto(result));
        }

        private static JObject ToDto(FindResponse<CaseFileDefinitionAggregate> resp)
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
                    { "payload", r.Payload },
                    { "create_datetime", r.CreateDateTime }
                })) }
            };
        }

        private static JObject ToDto(CaseFileDefinitionAggregate resp)
        {
            return new JObject
            {
                { "id", resp.Id },
                { "name", resp.Name },
                { "description", resp.Description },
                { "payload", resp.Payload },
                { "create_datetime", resp.CreateDateTime }
            };
        }

        private static FindCaseDefinitionFilesParameter ExtractFindParameter(IEnumerable<KeyValuePair<string, string>> query)
        {
            int startIndex;
            int count;
            string orderBy;
            FindOrders findOrder;
            var parameter = new FindCaseDefinitionFilesParameter();
            if (query.TryGet("start_index", out startIndex))
            {
                parameter.StartIndex = startIndex;
            }

            if (query.TryGet("count", out count))
            {
                parameter.Count = count;
            }

            if (query.TryGet("order_by", out orderBy))
            {
                parameter.OrderBy = orderBy;
            }

            if (query.TryGet("order", out findOrder))
            {
                parameter.Order = findOrder;
            }

            return parameter;
        }
    }
}