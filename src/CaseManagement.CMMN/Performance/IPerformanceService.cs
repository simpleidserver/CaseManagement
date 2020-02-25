using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Performance
{
    public interface IPerformanceService
    {
        Task<JArray> GetPerformances();
        Task<JObject> SearchPerformances(IEnumerable<KeyValuePair<string, string>> query);
    }
}
