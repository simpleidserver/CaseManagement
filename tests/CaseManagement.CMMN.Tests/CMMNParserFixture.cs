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
            var firstPath = Path.Combine(Directory.GetCurrentDirectory(), "Cmmns", "CaseWithOneTask.cmmn");
            var secondPath = Path.Combine(Directory.GetCurrentDirectory(), "Cmmns", "CaseWithTwoStages.cmmn");
            var firstResult = CMMNParser.ExtractWorkflowDefinition(firstPath);
            var secondResult = CMMNParser.ExtractWorkflowDefinition(secondPath);
            Assert.NotNull(firstResult);
            Assert.NotNull(secondResult);
        }
    }
}