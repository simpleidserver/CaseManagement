using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class StageElementInstance : BaseTaskOrStageElementInstance
    {
        public StageElementInstance()
        {
            Children = new List<BaseCasePlanItemInstance>();
        }

        #region Properties

        public override CasePlanElementInstanceTypes Type { get => CasePlanElementInstanceTypes.STAGE; }
        public ICollection<BaseCasePlanItemInstance> Children { get; set; }
        public int Length => Children.Count();

        #endregion

        #region Get or set operations

        public void AddChild(BaseCasePlanItemInstance child)
        {
            Children.Add(child);
        }

        public BaseCasePlanItemInstance GetChild(string id)
        {
            var child = Children.FirstOrDefault(_ => _.Id == id);
            if (child != null)
            {
                return child;
            }

            var stages = Children.Where(_ => _ is StageElementInstance).Select(_ => _ as StageElementInstance);
            foreach (var stage in stages)
            {
                child = stage.GetChild(id);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }

        public StageElementInstance GetParent(string id)
        {
            if (Children.Any(c => c.Id == id))
            {
                return this;
            }

            var stages = Children.Where(c => c is StageElementInstance).Cast<StageElementInstance>();
            foreach(var stage in stages)
            {
                var result = stage.GetParent(id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public List<BaseCasePlanItemInstance> GetFlatListChildren()
        {
            var result = new List<BaseCasePlanItemInstance> { this };
            foreach(var child in Children)
            {
                var stage = child as StageElementInstance;
                if (stage != null)
                {
                    result.AddRange(stage.GetFlatListChildren());
                }
                else
                {
                    result.Add(child);
                }
            }

            return result;
        }

        #endregion

        public override object Clone()
        {
            var result = new StageElementInstance
            {
                Children = Children.Select(_ => (BaseCasePlanItemInstance)_.Clone()).ToList()
            };
            FeedCaseEltInstance(result);
            FeedTaskOrStage(result);
            return result;
        }

        public static StageElementInstance FromJson(string json)
        {
            var elt = FromJsonElt(json);
            return elt as StageElementInstance;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        private static BaseCasePlanItemInstance FromJsonElt(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<JObject>(json);
            var type = deserialized["Type"].ToString();
            var typeEnum = (CasePlanElementInstanceTypes)Enum.Parse(typeof(CasePlanElementInstanceTypes), type);
            switch (typeEnum)
            {
                case CasePlanElementInstanceTypes.HUMANTASK:
                    return JsonConvert.DeserializeObject<HumanTaskElementInstance>(json);
                case CasePlanElementInstanceTypes.STAGE:
                    var chl = new List<BaseCasePlanItemInstance>();
                    var children = deserialized["Children"] as JArray;
                    foreach (var child in children)
                    {
                        chl.Add(FromJsonElt(child.ToString()));
                    }

                    deserialized.Remove("Children");
                    var stage = JsonConvert.DeserializeObject<StageElementInstance>(deserialized.ToString());
                    stage.Children = chl;                
                    return stage;
                case CasePlanElementInstanceTypes.EMPTYTASK:
                    return JsonConvert.DeserializeObject<EmptyTaskElementInstance>(json);
                case CasePlanElementInstanceTypes.MILESTONE:
                    return JsonConvert.DeserializeObject<MilestoneElementInstance>(json);
                case CasePlanElementInstanceTypes.TIMER:
                    return JsonConvert.DeserializeObject<TimerEventListener>(json);
            }

            return null;
        }

        public override BaseCasePlanItemInstance NewOccurrence(string casePlanInstanceId)
        {
            var clone = Clone() as ProcessTaskElementInstance;
            clone.State = null;
            clone.TransitionHistories = new ConcurrentBag<CasePlanElementInstanceTransitionHistory>();
            clone.NbOccurrence = NbOccurrence + 1;
            clone.Id = BuildId(casePlanInstanceId, EltId, clone.NbOccurrence);
            clone.EntryCriterions = CloneEntryCriterions();
            clone.ExitCriterions = CloneExitCriterions();
            return clone;
        }
    }
}
