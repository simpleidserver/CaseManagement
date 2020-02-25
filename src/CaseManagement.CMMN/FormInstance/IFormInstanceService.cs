using CaseManagement.CMMN.FormInstance.Commands;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.FormInstance
{
    public interface IFormInstanceService
    {
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query);
        Task Confirm(ConfirmFormInstanceCommand cmd);
    }
}
