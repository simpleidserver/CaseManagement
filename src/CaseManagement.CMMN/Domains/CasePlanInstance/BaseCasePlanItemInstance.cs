using CaseManagement.Common.Processors;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    public abstract class BaseCasePlanItemInstance : BaseCaseEltInstance, IBranchNode
    {
        public BaseCasePlanItemInstance() : base()
        {
            EntryCriterions = new List<Criteria>();
            ExitCriterions = new List<Criteria>();
        }

        #region Properties

        public int NbOccurrence { get; set; }
        public RepetitionRule RepetitionRule { get; set; }
        public ICollection<Criteria> EntryCriterions { get; set; }
        public ICollection<Criteria> ExitCriterions { get; set; }
        public ICollection<string> Incoming => EntryCriterions.SelectMany(ec => ec.SEntry.PlanItemOnParts.Select(pp => pp.SourceRef)).ToList();

        #endregion

        public bool IsLeaf()
        {
            return EntryCriterions == null || !EntryCriterions.Any() || EntryCriterions.All(ec => ec.SEntry == null || !ec.SEntry.PlanItemOnParts.Any());
        }

        protected void FeedCasePlanItem(BaseCasePlanItemInstance elt)
        {
            elt.NbOccurrence = NbOccurrence;
            elt.RepetitionRule = (RepetitionRule)RepetitionRule?.Clone();
            elt.EntryCriterions = EntryCriterions.Select(_ => (Criteria)_.Clone()).ToList();
            elt.ExitCriterions = ExitCriterions.Select(_ => (Criteria)_.Clone()).ToList();
        }

        public static string BuildId(string casePlanInstanceId, string eltId, int nbOccurrence)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{casePlanInstanceId}{eltId}{nbOccurrence}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public abstract BaseCasePlanItemInstance NewOccurrence(string casePlanInstanceId);

        protected ICollection<Criteria> CloneEntryCriterions()
        {
            return EntryCriterions.Select(_ => CloneCriteria(_)).ToList();
        }

        protected ICollection<Criteria> CloneExitCriterions()
        {
            return ExitCriterions.Select(_ => CloneCriteria(_)).ToList();
        }

        private Criteria CloneCriteria(Criteria criteria)
        {
            var result = new Criteria(criteria.Name);
            if (criteria.SEntry != null)
            {
                result.SEntry = new SEntry(criteria.SEntry.Name);
                foreach(var fileItemOnPart in criteria.SEntry.FileItemOnParts)
                {
                    result.SEntry.FileItemOnParts.Add(new CaseFileItemOnPart
                    {
                        SourceRef = fileItemOnPart.SourceRef,
                        StandardEvent = fileItemOnPart.StandardEvent
                    });
                }

                foreach (var planItemOnPart in criteria.SEntry.PlanItemOnParts)
                {
                    result.SEntry.PlanItemOnParts.Add(new PlanItemOnPart
                    {
                        SourceRef = planItemOnPart.SourceRef,
                        StandardEvent = planItemOnPart.StandardEvent
                    });
                }

                if (criteria.SEntry.IfPart != null)
                {
                    result.SEntry.IfPart = new IfPart
                    {
                        Condition = criteria.SEntry.IfPart.Condition
                    };
                }
            }

            return result;
        }
    }
}
