namespace CaseManagement.BPMN
{
    public class BPMNServerOptions
    {
        public BPMNServerOptions()
        {
            DefaultBPMNFile = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
"<definitions xmlns=\"http://www.omg.org/spec/BPMN/20100524/MODEL\" xmlns:bpmndi=\"http://www.omg.org/spec/BPMN/20100524/DI\" xmlns:omgdi=\"http://www.omg.org/spec/DD/20100524/DI\" xmlns:omgdc=\"http://www.omg.org/spec/DD/20100524/DC\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" id=\"sid-38422fae-e03e-43a3-bef4-bd33b32041b2\" targetNamespace=\"http://bpmn.io/bpmn\" exporter=\"bpmn-js (https://demo.bpmn.io)\" exporterVersion=\"7.4.1\">" +
"  <process id=\"Process_1\" isExecutable=\"false\">" +
"    <startEvent id=\"StartEvent_1y45yut\" name=\"hunger noticed\">" +
"      <outgoing>SequenceFlow_0h21x7r</outgoing>" +
"    </startEvent>" +
"    <task id=\"Task_1hcentk\" name=\"choose recipe\">" +
"      <incoming>SequenceFlow_0h21x7r</incoming>" +
"      <outgoing>SequenceFlow_0wnb4ke</outgoing>" +
"    </task>" +
"    <sequenceFlow id=\"SequenceFlow_0h21x7r\" sourceRef=\"StartEvent_1y45yut\" targetRef=\"Task_1hcentk\" />" +
"    <exclusiveGateway id=\"ExclusiveGateway_15hu1pt\" name=\"desired dish?\">" +
"      <incoming>SequenceFlow_0wnb4ke</incoming>" +
"    </exclusiveGateway>" +
"    <sequenceFlow id=\"SequenceFlow_0wnb4ke\" sourceRef=\"Task_1hcentk\" targetRef=\"ExclusiveGateway_15hu1pt\" />" +
"  </process>" +
"  <bpmndi:BPMNDiagram id=\"BpmnDiagram_1\">" +
"    <bpmndi:BPMNPlane id=\"BpmnPlane_1\" bpmnElement=\"Process_1\">" +
"      <bpmndi:BPMNEdge id=\"SequenceFlow_0wnb4ke_di\" bpmnElement=\"SequenceFlow_0wnb4ke\">" +
"        <omgdi:waypoint x=\"470\" y=\"120\" />" +
"        <omgdi:waypoint x=\"675\" y=\"120\" />" +
"      </bpmndi:BPMNEdge>" +
"      <bpmndi:BPMNEdge id=\"SequenceFlow_0h21x7r_di\" bpmnElement=\"SequenceFlow_0h21x7r\">" +
"        <omgdi:waypoint x=\"208\" y=\"120\" />" +
"        <omgdi:waypoint x=\"370\" y=\"120\" />" +
"      </bpmndi:BPMNEdge>" +
"      <bpmndi:BPMNShape id=\"ExclusiveGateway_15hu1pt_di\" bpmnElement=\"ExclusiveGateway_15hu1pt\" isMarkerVisible=\"true\">" +
"        <omgdc:Bounds x=\"675\" y=\"95\" width=\"50\" height=\"50\" />" +
"        <bpmndi:BPMNLabel>" +
"          <omgdc:Bounds x=\"668\" y=\"152\" width=\"66\" height=\"14\" />" +
"        </bpmndi:BPMNLabel>" +
"      </bpmndi:BPMNShape>" +
"      <bpmndi:BPMNShape id=\"Task_1hcentk_di\" bpmnElement=\"Task_1hcentk\">" +
"        <omgdc:Bounds x=\"370\" y=\"80\" width=\"100\" height=\"80\" />" +
"      </bpmndi:BPMNShape>" +
"      <bpmndi:BPMNShape id=\"StartEvent_1y45yut_di\" bpmnElement=\"StartEvent_1y45yut\">" +
"        <omgdc:Bounds x=\"172\" y=\"102\" width=\"36\" height=\"36\" />" +
"        <bpmndi:BPMNLabel>" +
"          <omgdc:Bounds x=\"154\" y=\"145\" width=\"74\" height=\"14\" />" +
"        </bpmndi:BPMNLabel>" +
"      </bpmndi:BPMNShape>" +
"    </bpmndi:BPMNPlane>" +
"  </bpmndi:BPMNDiagram>" +
"</definitions>";
            OAuthTokenEndpoint = "https://localhost:60000/token";
            ClientId = "bpmnClient";
            ClientSecret = "bpmnClientSecret";
        }

        public string WSHumanTaskAPI { get; set; }
        public string DefaultBPMNFile { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string OAuthTokenEndpoint { get; set; }
        public string CallbackUrl { get; set; }
    }
}
