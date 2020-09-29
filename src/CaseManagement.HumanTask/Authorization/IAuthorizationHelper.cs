using CaseManagement.HumanTask.Domains;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Authorization
{
    public interface IAuthorizationHelper
    {
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
        /// <param name="assignment"></param>
        /// <param name="claims"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ICollection<UserRoles>> GetRoles(TaskPeopleAssignment assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token);
    }
}
