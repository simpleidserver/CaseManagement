using CaseManagement.CMMN.Parser;
using System.IO;
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
            string ss = "";
        }
    }
}
