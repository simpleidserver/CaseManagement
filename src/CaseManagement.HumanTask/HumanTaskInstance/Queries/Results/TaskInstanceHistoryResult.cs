using CaseManagement.HumanTask.Domains;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CaseManagement.HumanTask.HumanTaskInstance.Queries.Results
{
    public class TaskInstanceHistoryResult
    {
        public string EventId { get; set; }
        public DateTime EventTime { get; set; }
        public string HumanTaskIdentifier { get; set; }
        public string UserPrincipal { get; set; }
        public string EventType { get; set; }
        public JObject EventData { get; set; }
        public string StartOwner { get; set; }
        public string EndOwner { get; set; }
        public string TaskStatus { get; set; }

        public static TaskInstanceHistoryResult ToDto(HumanTaskInstanceEventHistory history, bool includeData = false)
        {
            var result = new TaskInstanceHistoryResult
            {
                EndOwner = history.EndOwner,
                EventId = history.EventId,
                EventTime = history.EventTime,
                EventType = Enum.GetName(typeof(HumanTaskInstanceEventTypes), history.EventType),
                HumanTaskIdentifier = history.HumanTaskIdentifier,
                StartOwner = history.StartOwner,
                UserPrincipal = history.UserPrincipal,
                TaskStatus = Enum.GetName(typeof(HumanTaskInstanceStatus), history.TaskStatus)
            };
            if (includeData && !string.IsNullOrWhiteSpace(history.EventData))
            {
                result.EventData = JsonConvert.DeserializeObject<JObject>(history.EventData);
            }

            return result;
        }
    }
}
