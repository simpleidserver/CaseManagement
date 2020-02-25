using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.DailyStatistic
{
    public interface IDailyStatisticService
    {
        Task<JObject> Get();
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query);
    }
}
