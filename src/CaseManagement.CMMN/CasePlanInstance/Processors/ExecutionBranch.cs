using CaseManagement.CMMN.Domains;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class ExecutionBranch
    {
        public ExecutionBranch(int level)
        {
            Nodes = new List<BaseCasePlanItemInstance>();
            Level = level;
        }

        public int Level { get; private set; }
        public ExecutionBranch NextBranch { get; set; }
        public ICollection<BaseCasePlanItemInstance> Nodes { get; internal set; }

        public void AddNode(BaseCasePlanItemInstance casePlanElementInstance)
        {
            Nodes.Add(casePlanElementInstance);
        }

        public ExecutionBranch BuildNextBranch()
        {
            var result = new ExecutionBranch(Level + 1);
            NextBranch = result;
            return result;
        }

        public bool IsRoot()
        {
            return Level == 0;
        }

        public BaseCasePlanItemInstance GetNode(BaseCasePlanItemInstance casePlanElementInstance)
        {
            var node = Nodes.FirstOrDefault(_ => _.Id == casePlanElementInstance.Id);
            if (node != null)
            {
                return node;
            }

            if (NextBranch != null)
            {
                return NextBranch.GetNode(casePlanElementInstance);
            }

            return null;
        }

        public static ExecutionBranch Build(ICollection<BaseCasePlanItemInstance> casePlanElementInstances, ExecutionBranch parentExecutionBranch = null)
        {
            IEnumerable<BaseCasePlanItemInstance> filteredCasePlanElementsInstances;
            if (parentExecutionBranch == null)
            {
                parentExecutionBranch = new ExecutionBranch(0);
                filteredCasePlanElementsInstances = casePlanElementInstances.Where(_ => parentExecutionBranch.IsRoot() && _.EntryCriterions == null || !_.EntryCriterions.Any());
            }
            else
            {
                filteredCasePlanElementsInstances = casePlanElementInstances.Where(_ =>
                {
                    var ids = _.EntryCriterions.SelectMany(ec => ec.SEntry.PlanItemOnParts.Select(pp => pp.SourceRef));
                    return !casePlanElementInstances.Any(i => ids.Contains(i.Id));
                });
            }

            foreach (var elt in filteredCasePlanElementsInstances)
            {
                parentExecutionBranch.AddNode(elt);
            }

            var removedIds = filteredCasePlanElementsInstances.Select(_ => _.Id);
            var newCasePlanElementInstances = casePlanElementInstances.Where(_ => !removedIds.Contains(_.Id)).ToList();
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
