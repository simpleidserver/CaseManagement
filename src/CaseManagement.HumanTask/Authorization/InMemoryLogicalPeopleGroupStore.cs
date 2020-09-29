using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.HumanTask.Authorization
{
    public class InMemoryLogicalPeopleGroupStore : ILogicalPeopleGroupStore
    {
        private ConcurrentBag<LogicalPeopleGroup> _logicalPeopleGroups;

        public InMemoryLogicalPeopleGroupStore(ConcurrentBag<LogicalPeopleGroup> logicalPeopleGroups)
        {
            _logicalPeopleGroups = logicalPeopleGroups;
        }

        /// <summary>
        /// Get members of a given logical people group.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="parameters"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<ICollection<Member>> GetMembers(string groupName, ICollection<KeyValuePair<string, string>> parameters, CancellationToken token)
        {
            ICollection<Member> result = new List<Member>();
            var logicalPeopleGroups = _logicalPeopleGroups.Where(_ => _.Name == groupName);
            if (!logicalPeopleGroups.Any())
            {
                return Task.FromResult(result);
            }

            if (parameters.Any())
            {
                logicalPeopleGroups = logicalPeopleGroups.Where(_ => parameters.All(p => _.Metadata.Any(m => m.Key == p.Key && m.Value == p.Value)));
            }

            result = logicalPeopleGroups.SelectMany(_ => _.Members).ToList();
            return Task.FromResult(result);
        }
    }
}
