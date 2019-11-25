// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace CaseManagement.CMMN.Acceptance.Tests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class CaseDefinitionsErrorsFeature : Xunit.IClassFixture<CaseDefinitionsErrorsFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "CaseDefinitionsErrors.feature"
#line hidden
        
        public CaseDefinitionsErrorsFeature(CaseDefinitionsErrorsFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CaseDefinitionsErrors", "\tCheck errors returned by /case-definitions", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Check error is returned when case-definition doesn\'t exist")]
        [Xunit.TraitAttribute("FeatureTitle", "CaseDefinitionsErrors")]
        [Xunit.TraitAttribute("Description", "Check error is returned when case-definition doesn\'t exist")]
        public virtual void CheckErrorIsReturnedWhenCase_DefinitionDoesntExist()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Check error is returned when case-definition doesn\'t exist", null, ((string[])(null)));
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.When("execute HTTP GET request \'http://localhost/case-definitions/invalid\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 7
 testRunner.Then("HTTP status code equals to \'404\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                CaseDefinitionsErrorsFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                CaseDefinitionsErrorsFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
