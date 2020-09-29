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

        public Task<ICollection<UserRoles>> GetRoles(HumanTaskInstanceAggregate humanTaskInstance, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            return GetRoles(humanTaskInstance.PeopleAssignment, claims, token);
        }

        public Task<ICollection<UserRoles>> GetRoles(TaskPeopleAssignment assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            ICollection<UserRoles> result = new List<UserRoles>();
            var cb = new Action<PeopleAssignment, UserRoles, ICollection<UserRoles>>(async (pas, ur, r) =>
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

            if (assignment == null)
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

        protected virtual Task<bool> IsAuthorized(PeopleAssignment assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            switch (assignment.Type)
            {
                case PeopleAssignmentTypes.EXPRESSION:
                    return IsAuthorized((ExpressionAssignment)assignment, claims, token);
                case PeopleAssignmentTypes.GROUPNAMES:
                    return IsAuthorized((GroupNamesAssignment)assignment, claims, token);
                case PeopleAssignmentTypes.LOGICALPEOPLEGROUP:
                    return IsAuthorized((LogicalPeopleGroupAssignment)assignment, claims, token);
                case PeopleAssignmentTypes.USERIDENTFIERS:
                    return IsAuthorized((UserIdentifiersAssignment)assignment, claims, token);
            }

            return Task.FromResult(false);
        }
        
        /// <summary>
        /// Check logical people group.
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        protected virtual async Task<bool> IsAuthorized(LogicalPeopleGroupAssignment assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            var members = await _logicalPeopleGroupStore.GetMembers(assignment.LogicalPeopleGroup, new List<KeyValuePair<string,string>>(), token);
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
        /// Check expression.
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        protected virtual Task<bool> IsAuthorized(ExpressionAssignment assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            // expression ???
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check group names.
        /// </summary>
        /// <param name="assignment"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        protected virtual Task<bool> IsAuthorized(GroupNamesAssignment assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            var roles = claims.Where(_ => _.Key == ClaimTypes.Role);
            if (!roles.Any())
            {
                return Task.FromResult(false);
            }

            if (!assignment.GroupNames.Any(_ => roles.Any(r => r.Value == _)))
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
        protected virtual Task<bool> IsAuthorized(UserIdentifiersAssignment assignment, IEnumerable<KeyValuePair<string, string>> claims, CancellationToken token)
        {
            var nameIdentifier = claims.FirstOrDefault(_ => _.Key == ClaimTypes.NameIdentifier);
            if (nameIdentifier.IsEmpty())
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(assignment.UserIdentifiers.Contains(nameIdentifier.Value));
        }

        #endregion
    }
}
