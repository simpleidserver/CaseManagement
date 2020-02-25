using CaseManagement.CMMN.CaseFile.Commands;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile
{
    public interface ICaseFileService
    {
        Task<JObject> Count();
        Task<JObject> Add(AddCaseFileCommand parameter);
        Task<bool> UpdateMe(UpdateCaseFileCommand parameter);
        Task<bool> Update(UpdateCaseFileCommand parameter);
        Task<JObject> PublishMe(PublishCaseFileCommand parameter);
        Task<JObject> Publish(PublishCaseFileCommand parameter);
        Task<JObject> GetMe(string id, string nameIdentifier);
        Task<JObject> Get(string id);
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query);
    }
}
