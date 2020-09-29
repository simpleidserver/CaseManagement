using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Authorization
{
    public interface ILogicalPeopleGroupStore
    {
        /// <summary>
        /// Get members of a given logical people group.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="parameters"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ICollection<Member>> GetMembers(string groupName, ICollection<KeyValuePair<string, string>> parameters, CancellationToken token);
    }
}
