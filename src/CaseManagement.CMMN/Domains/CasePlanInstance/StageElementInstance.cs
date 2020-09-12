using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class StageElementInstance : BaseTaskOrStageElementInstance
    {
        public StageElementInstance()
        {
            Children = new List<CasePlanElementInstance>();
        }

        public override string Type { get => "stage"; }
        public ICollection<CasePlanElementInstance> Children { get; set; }
        public int Length => Children.Count();

        public void AddChild(CasePlanElementInstance child)
        {
            Children.Add(child);
        }

        public CasePlanElementInstance GetChild(string id)
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

        public List<CasePlanElementInstance> GetFlatListChildren()
        {
            var result = new List<CasePlanElementInstance> { this };
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

        public override object Clone()
        {
            var result = new StageElementInstance
            {
                Children = Children.Select(_ => (CasePlanElementInstance)_.Clone()).ToList()
            };
            FeedCasePlanElement(result);
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

        private static CasePlanElementInstance FromJsonElt(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<JObject>(json);
            var type = deserialized["Type"].ToString();
            switch (type)
            {
                case "humantask":
                    return JsonConvert.DeserializeObject<HumanTaskElementInstance>(json);
                case "stage":
                    var chl = new List<CasePlanElementInstance>();
                    var children = deserialized["Children"] as JArray;
                    foreach (var child in children)
                    {
                        chl.Add(FromJsonElt(child.ToString()));
                    }

                    deserialized.Remove("Children");
                    var stage = JsonConvert.DeserializeObject<StageElementInstance>(deserialized.ToString());
                    stage.Children = chl;                
                    return stage;
                case "fileitem":
                    return JsonConvert.DeserializeObject<CaseFileItemInstance>(json);
                case "emptytask":
                    return JsonConvert.DeserializeObject<EmptyTaskElementInstance>(json);
                case "milestone":
                    return JsonConvert.DeserializeObject<MilestoneElementInstance>(json);
                case "timer":
                    return JsonConvert.DeserializeObject<TimerEventListener>(json);
            }

            return null;
        }
    }
}
