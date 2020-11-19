using CaseManagement.BPMN.Builders;
using CaseManagement.BPMN.Domains;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CaseManagement.BPMN.Parser
{
    public class BPMNParser
    {
        private static Dictionary<tGatewayDirection, GatewayDirections> MAPPING_GATEWAY_DIRECTIONS = new Dictionary<tGatewayDirection, GatewayDirections>
        {
            { tGatewayDirection.Converging, GatewayDirections.CONVERGING },
            { tGatewayDirection.Diverging, GatewayDirections.DIVERGING },
            { tGatewayDirection.Mixed, GatewayDirections.MIXED },
            { tGatewayDirection.Unspecified, GatewayDirections.UNSPECIFIED }
        };

        public static tDefinitions Parse(string bpmnTxt)
        {
            var xmlSerializer = new XmlSerializer(typeof(tDefinitions), "http://www.omg.org/spec/BPMN/20100524/MODEL");
            tDefinitions defs = null;
            using (var txtReader = new StringReader(bpmnTxt))
            {
                defs = (tDefinitions)xmlSerializer.Deserialize(txtReader);
            }

            return defs;
        }

        public static ICollection<ProcessInstanceAggregate> BuildInstances(string bpmnTxt, string processFileId)
        {
            var tDefinitions = Parse(bpmnTxt);
            return BuildInstances(tDefinitions, processFileId);
        }

        public static ICollection<ProcessInstanceAggregate> BuildInstances(tDefinitions definitions, string processFileId)
        {
            var result = new List<ProcessInstanceAggregate>();
            var processes = definitions.Items.Where(_ => _ is tProcess).Cast<tProcess>();
            foreach(var process in processes)
            {
                var builder = ProcessInstanceBuilder.New(processFileId);
                foreach(var item in process.Items)
                {
                    tStartEvent startEvt;
                    tTask task;
                    tSequenceFlow sequenceFlow;
                    tExclusiveGateway exclusiveGateway;
                    tUserTask userTask;
                    if ((startEvt = item as tStartEvent) != null)
                    {
                        builder.AddStartEvent(startEvt.id, startEvt.name);
                    }
                    else if ((userTask = item as tUserTask) != null)
                    {
                        builder.AddUserTask(userTask.id, userTask.name, (cb) =>
                        {
                            if (userTask.implementation == BPMNConstants.UserTaskImplementations.WSHUMANTASK)
                            {
                                cb.SetWsHumanTask(userTask.wsHumanTaskDefName);
                            }
                        });
                    }
                    else if((task = item as tTask) != null)
                    {
                        builder.AddEmptyTask(task.id, task.name);
                    }
                    else if((sequenceFlow = item as tSequenceFlow) != null)
                    {
                        builder.AddSequenceFlow(sequenceFlow.id, sequenceFlow.name, sequenceFlow.sourceRef, sequenceFlow.targetRef, sequenceFlow.conditionExpression?.Text.First());
                    }
                    else if((exclusiveGateway = item as tExclusiveGateway) != null)
                    {
                        builder.AddExclusiveGateway(exclusiveGateway.id, exclusiveGateway.name, MAPPING_GATEWAY_DIRECTIONS[exclusiveGateway.gatewayDirection], exclusiveGateway.@default);
                    }
                }

                result.Add(builder.Build());
            }

            return result;
        }
    }
}
