namespace CaseManagement.BPMN.Parser
{
    public interface IBPMNParser
    {
        BPMNParsed ParseWSDL(string bpmnTxt, string dataDefsTxt, string interfacesTxt);
    }
}
