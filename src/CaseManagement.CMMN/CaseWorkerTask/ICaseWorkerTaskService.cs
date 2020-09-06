using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseWorkerTask
{
    public interface ICaseWorkerTaskService
    {
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query);
    }
}
