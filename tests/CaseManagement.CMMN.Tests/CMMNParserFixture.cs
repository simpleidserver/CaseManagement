using CaseManagement.CMMN.Domains;
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
            var fifthPath = Path.Combine(Directory.GetCurrentDirectory(), "Cmmns", "CaseWithOneDiscretionaryItem.cmmn");
            var caseFile = new CaseFileAggregate();
            var firstResult = CMMNParser.ExtractCasePlans(CMMNParser.ParseWSDL(File.ReadAllText(firstPath)), caseFile);
            var secondResult = CMMNParser.ExtractCasePlans(CMMNParser.ParseWSDL(File.ReadAllText(secondPath)), caseFile);
            var thirdResult = CMMNParser.ExtractCasePlans(CMMNParser.ParseWSDL(File.ReadAllText(thirdPath)), caseFile);
            var fourthResult = CMMNParser.ExtractCasePlans(CMMNParser.ParseWSDL(File.ReadAllText(fourthPath)), caseFile);
            var fifthResult = CMMNParser.ExtractCasePlans(CMMNParser.ParseWSDL(File.ReadAllText(fifthPath)), caseFile);
            Assert.NotNull(firstResult);
            Assert.NotNull(secondResult);
            Assert.NotNull(thirdResult);
            Assert.NotNull(fourthResult);
            Assert.NotNull(fifthResult);
        }
    }
}