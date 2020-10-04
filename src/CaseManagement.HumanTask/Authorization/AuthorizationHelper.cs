using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Authorization
{
    public class AuthorizationHelper : IAuthorizationHelper
    {
        private readonly ILogicalPeopleGroupStore _logicalPeopleGroupStore;

        public AuthorizationHelper(ILogicalPeopleGroupStore logicalPeopleGroupStore)
        {
            _logicalPeopleGroupStore = logicalPeopleGroupStore;
        }

        public string GetNameIdentifier(IEnumerable<KeyValuePair<string, string>> claims)
        {
            var nameIdentifier = claims.FirstOrDefault(_ => _.Key == ClaimTypes.NameIdentifier);
            if (nameIdentifier.IsEmpty())
            {
                return null;
            }

            return nameIdentifier.Value;
        }

        public ICollection<string> GetRoles(IEnumerable<KeyValuePair<string, string>> claims)
        {
            return claims.Where(_ => _.Key == ClaimTypes.Role).Select(_ => _.Value).ToList();
        }

        public Task<ICollection<UserRoles>> GetRoles(HumanTaskInstanceAggregate humanTaskInstance, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            return GetRoles(humanTaskInstance.PeopleAssignment, claims, token);
        }

        public Task<ICollection<UserRoles>> GetRoles(HumanTaskInstancePeopleAssignment assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            ICollection<UserRoles> result = new List<UserRoles>();
            var cb = new Action<PeopleAssignmentInstance, UserRoles, ICollection<UserRoles>>(async (pas, ur, r) =>
            {
                if (pas == null)
                {
                    return;
                }

                if (await IsAuthorized(pas, claims, token))
                {
                    r.Add(ur);
                }
            });

            if (assignment != null)
            {
                cb(assignment.BusinessAdministrator, UserRoles.BUSINESSADMINISTRATOR, result);
                cb(assignment.ExcludedOwner, UserRoles.EXCLUDEDOWNER, result);
                cb(assignment.PotentialOwner, UserRoles.POTENTIALOWNER, result);
                cb(assignment.TaskInitiator, UserRoles.TASKINITIATOR, result);
                cb(assignment.TaskStakeHolder, UserRoles.TASKSTAKEHODLER, result);
            }

            return Task.FromResult(result);
        }

        #region Check authorization

        protected virtual Task<bool> IsAuthorized(PeopleAssignmentInstance assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            switch (assignment.Type)
            {
                case PeopleAssignmentTypes.GROUPNAMES:
                    return CheckGroupNames(assignment.Values, claims, token);
                case PeopleAssignmentTypes.USERIDENTFIERS:
                    return CheckUserIdentifiers(assignment.Values, claims, token);
                case PeopleAssignmentTypes.LOGICALPEOPLEGROUP:
                    return CheckLogicalGroup(assignment.LogicalGroup, claims, token);
            }

            return Task.FromResult(false);
        }
        
        /// <summary>
        /// Check logical people group.
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        protected virtual async Task<bool> CheckLogicalGroup(LogicalGroupInstance assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            var members = await _logicalPeopleGroupStore.GetMembers(assignment.Name, new List<KeyValuePair<string,string>>(), token);
            foreach(var member in members)
            {
                if (member.Claims.All(c => claims.Any(cl => cl.Key == c.Key && cl.Value == c.Value)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check group names.
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        protected virtual Task<bool> CheckGroupNames(ICollection<string> groupNames, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            var roles = GetRoles(claims);
            if (!roles.Any())
            {
                return Task.FromResult(false);
            }

            if (!groupNames.Any(_ => roles.Any(r => r == _)))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Check user identifiers.
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        protected virtual Task<bool> CheckUserIdentifiers(ICollection<string> userIdentifiers, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            var nameIdentifier = GetNameIdentifier(claims);
            if (string.IsNullOrEmpty(nameIdentifier))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(userIdentifiers.Contains(nameIdentifier));
        }

        #endregion
    }
}
