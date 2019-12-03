using CaseManagement.CMMN.CaseInstance.CommandHandlers;
using CaseManagement.CMMN.Parser;
using CaseManagement.Workflow.Domains.Process.Exceptions;
using System.IO;
using System.Linq;
using Xunit;

namespace CaseManagement.Workflow.Tests
{
    public class ProcessFlowInstanceFixture
    {
        private readonly tDefinitions _cmmn;

        public ProcessFlowInstanceFixture()
        {
            var xml = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Cmmns", "sEntryWithCondition.cmmn"));
            _cmmn = new CMMNParser().ParseWSDL(xml);
        }

        [Fact]
        public void When_Launch_ProcessInstance_TwoTimes_Then_Exception_Is_Thrown()
        {
            var processFlowInstance = CreateCaseInstanceCommandHandler.BuildProcessFlowInstance(_cmmn.@case.First(), "sEntryWithCondition");
            processFlowInstance.Launch();
            var ex = Assert.Throws<ProcessFlowInstanceDomainException>(() => processFlowInstance.Launch());
            Assert.NotNull(ex);
            Assert.Equal("process instance is already launched", ex.Errors.First().Value);
        }
    }
}
