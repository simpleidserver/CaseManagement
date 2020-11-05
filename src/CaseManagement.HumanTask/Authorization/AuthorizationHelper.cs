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
            return GetRoles(humanTaskInstance.PeopleAssignments, claims, token);
        }

        public Task<ICollection<UserRoles>> GetRoles(ICollection<PeopleAssignmentInstance> peopleAssignments, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            ICollection<UserRoles> result = new List<UserRoles>();
            var cb = new Action<ICollection<PeopleAssignmentInstance>, UserRoles, ICollection<UserRoles>>(async (pas, ur, r) =>
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

            cb(peopleAssignments.GetBusinessAdministrators(), UserRoles.BUSINESSADMINISTRATOR, result);
            cb(peopleAssignments.GetExcludedOwners(), UserRoles.EXCLUDEDOWNER, result);
            cb(peopleAssignments.GetPotentialOwners(), UserRoles.POTENTIALOWNER, result);
            cb(peopleAssignments.GetTaskInitiators(), UserRoles.TASKINITIATOR, result);
            cb(peopleAssignments.GetTaskStakeHolders(), UserRoles.TASKSTAKEHODLER, result);
            return Task.FromResult(result);
        }

        #region Check authorization

        protected virtual async Task<bool> IsAuthorized(ICollection<PeopleAssignmentInstance> assignments, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            var nameIdentifier = GetNameIdentifier(claims);
            var roles = GetRoles(claims);
            foreach (var assignment in assignments)
            {
                switch(assignment.Type)
                {
                    case PeopleAssignmentTypes.USERIDENTIFIERS:
                        if (assignment.Value == nameIdentifier)
                        {
                            return true;
                        }
                        break;
                    case PeopleAssignmentTypes.GROUPNAMES:
                        if (roles.Contains(assignment.Value))
                        {
                            return true;
                        }
                        break;
                    case PeopleAssignmentTypes.LOGICALPEOPLEGROUP:
                        var members = await _logicalPeopleGroupStore.GetMembers(assignment.Value, new List<KeyValuePair<string, string>>(), token);
                        foreach (var member in members)
                        {
                            if (member.Claims.All(c => claims.Any(cl => cl.Key == c.Key && cl.Value == c.Value)))
                            {
                                return true;
                            }
                        }
                        break;
                }
            }

            return false;
        }

        #endregion
    }
}
