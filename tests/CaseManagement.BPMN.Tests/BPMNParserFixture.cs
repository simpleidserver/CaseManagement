using CaseManagement.BPMN.Parser;
using System.IO;
using Xunit;

namespace CaseManagement.BPMN.Tests
{
    public class BPMNParserFixture
    {
        [Fact]
        public void When_Parse_BPMN_File()
        {
            var bpmnTxt = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "diagram.bpmn"));
            var result = BPMNParser.Parse(bpmnTxt);
            Assert.NotNull(result);
        }

        [Fact]
        public void When_Parse_BPMN_File_And_Build_Instance()
        {
            var bpmnTxt = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "diagram.bpmn"));
            var result = BPMNParser.BuildInstances(bpmnTxt, "processfileid");
            Assert.NotNull(result);
        }
    }
}
