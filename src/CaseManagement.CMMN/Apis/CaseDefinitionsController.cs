using CaseManagement.CMMN.Persistence;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Apis
{
    [Route(CMMNConstants.RouteNames.CaseDefinitions)]
    public class CaseDefinitionsController : Controller
    {
        private readonly ICMMNDefinitionsQueryRepository _queryRepository;

        public CaseDefinitionsController(ICMMNDefinitionsQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _queryRepository.GetAll();
            return new OkObjectResult(new JArray(result.Select(r => new JObject
            {
                { "id", r.id },
                { "name", r.name },
                { "create_datetime", r.creationDate }
            })));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _queryRepository.FindDefinitionById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToDto(result));
        }

        [HttpGet("{id}/cases")]
        public async Task<IActionResult> GetCases(string id)
        {
            var result = await _queryRepository.FindDefinitionById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result.@case);
        }

        private static JObject ToDto(tDefinitions def)
        {
            var result = new JObject
            {
                { "id", def.id },
                { "name", def.name },
                { "create_datetime", def.creationDate },
            };
            result.Add("cases", ToDto(def.@case));
            return result;
        }

        private static JArray ToDto(tCase[] cases)
        {
            var cmmnCases = new JArray();
            foreach (var cmmnCase in cases)
            {
                cmmnCases.Add(new JObject
                {
                    { "id", cmmnCase.id },
                    { "name", cmmnCase.name }
                });
            }

            return cmmnCases;
        }
    }
}
