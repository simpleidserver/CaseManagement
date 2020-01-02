using CaseManagement.CMMN.Parser;
using System.IO;
using Xunit;

namespace CaseManagement.CMMN.Tests
{
    public class CMMNParserFixture
    {
        [Fact]
        public void When_Extract_WorkflowDefinitions()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Cmmns", "CaseWithOneTask.cmmn");
            var result = CMMNParser.ExtractWorkflowDefinition(path);
            Assert.NotNull(result);
        }
    }
}