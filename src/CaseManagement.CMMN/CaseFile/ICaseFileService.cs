using CaseManagement.CMMN.CaseFile.Commands;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CaseFile
{
    public interface ICaseFileService
    {
        Task<JObject> Count(CancellationToken cancellationToken);
        Task<JObject> Add(AddCaseFileCommand parameter, CancellationToken cancellationToken);
        Task<bool> Update(UpdateCaseFileCommand parameter, CancellationToken cancellationToken);
        Task<JObject> Publish(PublishCaseFileCommand parameter, CancellationToken cancellationToken);
        Task<JObject> Get(string id, CancellationToken cancellationToken);
        Task<JObject> Search(IEnumerable<KeyValuePair<string, string>> query, CancellationToken cancellationToken);
    }
}
