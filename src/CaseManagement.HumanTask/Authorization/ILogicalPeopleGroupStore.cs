using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Authorization
{
    public interface ILogicalPeopleGroupStore
    {
        /// <summary>
        /// Get logical groups of a user.
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ICollection<LogicalPeopleGroup>> GetLogicalGroups(IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token);
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
