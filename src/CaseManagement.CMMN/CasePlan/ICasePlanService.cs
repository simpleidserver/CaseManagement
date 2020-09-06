using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlan
{
    public interface ICasePlanService
    {
        Task<JObject> Count(CancellationToken token);
        Task<JObject> Get(string id, CancellationToken token);
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query, CancellationToken token);
    }
}
