using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Authorization
{
    public interface IAuthorizationHelper
    {
        /// <summary>
        /// Get name identifier.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string GetNameIdentifier(IEnumerable<KeyValuePair<string, string>> claims);
        /// <summary>
        /// Get roles.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        ICollection<string> GetRoles(IEnumerable<KeyValuePair<string, string>> claims);
        /// <summary>
        /// Get user roles.
        /// </summary>
        /// <param name="humanTaskInstance"></param>
        /// <param name="claims"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ICollection<UserRoles>> GetRoles(HumanTaskInstanceAggregate humanTaskInstance, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token);
        /// <summary>
        /// Get user roles.
        /// </summary>
        /// <param name="peopleAssignments"></param>
        /// <param name="claims"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ICollection<UserRoles>> GetRoles(ICollection<PeopleAssignmentInstance> peopleAssignments, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token);
    }
}
