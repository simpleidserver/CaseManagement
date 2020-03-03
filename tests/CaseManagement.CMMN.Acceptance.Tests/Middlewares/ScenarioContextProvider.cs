using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace CaseManagement.CMMN.Acceptance.Tests.Middlewares
{
    public interface IScenarioContextProvider
    {
        ScenarioContext GetContext();
        void SetContext(ScenarioContext context);
    }

    public class ScenarioContextProvider : IScenarioContextProvider
    {
        private ScenarioContext _scenarioContext;

        public ScenarioContext GetContext()
        {
            return _scenarioContext;
        }

        public void SetContext(ScenarioContext context)
        {
            _scenarioContext = context;
        }
    }
}
