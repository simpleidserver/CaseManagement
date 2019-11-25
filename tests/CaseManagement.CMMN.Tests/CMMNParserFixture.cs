using CaseManagement.CMMN.CaseInstance.Handlers;
using CaseManagement.CMMN.Parser;
using System.IO;
using System.Linq;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class CMMNParserFixture
    {
        [Fact]
        public void When_Deserialize_Bpmn()
        {
            var parser = new CMMNParser();
            var cmmnText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns", "diagram.cmmn"));
            var s = parser.ParseWSDL(cmmnText);
            var xml = parser.Serialize(s);
            var c = s.@case.First();
            var flowInstance = CreateCaseInstanceCommandHandler.BuildProcessFlowInstance(c);
            string ss = "";
        }
    }
}
