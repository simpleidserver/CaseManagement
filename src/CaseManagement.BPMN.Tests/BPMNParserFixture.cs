using CaseManagement.BPMN.Parser;
using System.IO;
using Xunit;

namespace CaseManagement.BPMN.Tests
{
    public class BPMNParserFixture
    {
        [Fact]
        public void When_Deserialize_Bpmn()
        {
            var parser = new BPMNParser();
            var bpmnEngine = new BPMNEngine();
            var bpmnText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Bpms", "CorrelationExampleSeller.bpmn"));
            var dataDefsText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Bpms", "DataDefinitions.xsd"));
            var interfacesText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Bpms", "Interfaces.wsdl"));
            var parsed = parser.ParseWSDL(bpmnText, dataDefsText, interfacesText);
            bpmnEngine.StartProcessInstanceOnReceiveTask("sellerProcess", "receiveQuoteRequest", parsed);
        }
    }
}
