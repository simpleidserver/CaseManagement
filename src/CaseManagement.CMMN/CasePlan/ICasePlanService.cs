using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan
{
    public interface ICasePlanService
    {
        Task<JObject> Count();
        Task<JObject> GetMe(string id, string nameIdentifier);
        Task<JObject> Get(string id);
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query);
    }
}
