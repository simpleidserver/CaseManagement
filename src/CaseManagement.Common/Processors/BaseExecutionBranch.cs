using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Common.Processors
{
    public class BaseExecutionBranch<T> where T : IBranchNode
    {
        public BaseExecutionBranch(int level)
        {
            Level = level;
            Nodes = new List<T>();
        }

        public int Level { get; private set; }
        public BaseExecutionBranch<T> NextBranch { get; set; }
        public ICollection<T> Nodes { get; internal set; }

        public void AddNode(T casePlanElementInstance)
        {
            Nodes.Add(casePlanElementInstance);
        }

        public BaseExecutionBranch<T> BuildNextBranch()
        {
            var result = new BaseExecutionBranch<T>(Level + 1);
            NextBranch = result;
            return result;
        }

        public bool IsRoot()
        {
            return Level == 0;
        }

        public static BaseExecutionBranch<T> Build(ICollection<T> flowNodes, BaseExecutionBranch<T> parentExecutionBranch = null)
        {
            IEnumerable<T> filteredFlowElts;
            if (parentExecutionBranch == null)
            {
                parentExecutionBranch = new BaseExecutionBranch<T>(0);
                filteredFlowElts = flowNodes.Where(_ => parentExecutionBranch.IsRoot() && _.IsLeaf());
            }
            else
            {
                filteredFlowElts = flowNodes.Where(_ =>
                {
                    var ids = _.Incoming;
                    return !flowNodes.Any(i => ids.Contains(i.Id));
                });
            }

            foreach (var elt in filteredFlowElts)
            {
                parentExecutionBranch.AddNode(elt);
            }

            var removedIds = filteredFlowElts.Select(_ => _.Id);
            var newCasePlanElementInstances = flowNodes.Where(_ => !removedIds.Contains(_.Id)).ToList();
            if (!newCasePlanElementInstances.Any())
            {
                return parentExecutionBranch;
            }

            var nextBranch = parentExecutionBranch.BuildNextBranch();
            Build(newCasePlanElementInstances, nextBranch);
            return parentExecutionBranch;
        }
    }
}
