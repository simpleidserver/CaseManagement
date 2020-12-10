using CaseManagement.HumanTask.Common;
using CaseManagement.HumanTask.Domains;
using CaseManagement.HumanTask.HumanTaskInstance.Commands;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Security.Claims;

namespace System.Collections.Generic
{
    public static class GenericCollectionExtensions
    {
        public static JObject ToJObj(this Dictionary<string, string> dic)
        {
            var result = new JObject();
            foreach(var kvp in dic)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }
        public static bool IsEmpty(this KeyValuePair<string, string> kvp)
        {
            if (kvp.Equals(default(KeyValuePair<string, string>)) || string.IsNullOrWhiteSpace(kvp.Value))
            {
                return true;
            }

            return false;
        }

        public static ICollection<string> GetGroupNames(this IEnumerable<KeyValuePair<string, string>> claims)
        {
            return claims.Where(_ => _.Key == ClaimTypes.Role).Select(_ => _.Value).ToList();
        }

        #region People assignments

        public static ICollection<PeopleAssignmentInstance> GetPotentialOwners(this ICollection<PeopleAssignmentInstance> peopleAssignments)
        {
            return peopleAssignments.Where(_ => _.Usage == PeopleAssignmentUsages.POTENTIALOWNER).ToList();
        }

        public static ICollection<PeopleAssignmentInstance> GetBusinessAdministrators(this ICollection<PeopleAssignmentInstance> peopleAssignments)
        {
            return peopleAssignments.Where(_ => _.Usage == PeopleAssignmentUsages.BUSINESSADMINISTRATOR).ToList();
        }

        public static ICollection<PeopleAssignmentInstance> GetExcludedOwners(this ICollection<PeopleAssignmentInstance> peopleAssignments)
        {
            return peopleAssignments.Where(_ => _.Usage == PeopleAssignmentUsages.EXCLUDEDOWNER).ToList();
        }

        public static ICollection<PeopleAssignmentInstance> GetTaskInitiators(this ICollection<PeopleAssignmentInstance> peopleAssignments)
        {
            return peopleAssignments.Where(_ => _.Usage == PeopleAssignmentUsages.TASKINITIATOR).ToList();
        }

        public static ICollection<PeopleAssignmentInstance> GetTaskStakeHolders(this ICollection<PeopleAssignmentInstance> peopleAssignments)
        {
            return peopleAssignments.Where(_ => _.Usage == PeopleAssignmentUsages.TASKSTAKEHOLDER).ToList();
        }

        #endregion
    }
}
