using CaseManagement.CMMN.Domains;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Form
{
    public interface IFormService
    {
        Task<JObject> Get(string id);
        Task<FormAggregate> GetForm(string id);
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query);
        Task<JObject> Get(string formId, int version);
    }
}
