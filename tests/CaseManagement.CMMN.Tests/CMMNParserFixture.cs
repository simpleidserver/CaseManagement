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
            var thirdPath = Path.Combine(Directory.GetCurrentDirectory(), "Cmmns", "CaseWithOneManualActivationTask.cmmn");
            var fourthPath = Path.Combine(Directory.GetCurrentDirectory(), "Cmmns", "CaseWithOneSEntry.cmmn");
            var firstResult = CMMNParser.ExtractWorkflowDefinition(firstPath);
            var secondResult = CMMNParser.ExtractWorkflowDefinition(secondPath);
            var thirdResult = CMMNParser.ExtractWorkflowDefinition(thirdPath);
            var fourthResult = CMMNParser.ExtractWorkflowDefinition(fourthPath);
            Assert.NotNull(firstResult);
            Assert.NotNull(secondResult);
            Assert.NotNull(thirdResult);
            Assert.NotNull(fourthResult);
        }
    }
}