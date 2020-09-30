using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace CaseManagement.HumanTasks.Acceptance.Tests.Steps
{
    [Binding]
    public class ValidationSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public ValidationSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }


        [Then("HTTP status code equals to '(.*)'")]
        public void ThenCheckHttpStatusCode(int code)
        {
            var httpResponseMessage = _scenarioContext["httpResponseMessage"] as HttpResponseMessage;
            Assert.Equal(code, (int)httpResponseMessage.StatusCode);
        }

        [Then("html = '(.*)'")]
        public async Task ThenHtmlEquals(string html)
        {
            var httpResponseMessage = _scenarioContext["httpResponseMessage"] as HttpResponseMessage;
            var h = await httpResponseMessage.Content.ReadAsStringAsync();
            Assert.Equal(h, html);
        }

        [Then("JSON contains '(.*)'")]
        public void ThenExists(string key)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            Assert.True(jsonHttpBody.ContainsKey(key) == true);
        }

        [Then("extract JSON '(.*)', JSON '(.*)'='(.*)'")]
        public void ThenExtractJSONEqualsTo(string jsonKey, string key, string value)
        {
            var jsonHttpBody = _scenarioContext[jsonKey] as JObject;
            var currentValue = jsonHttpBody.SelectToken(key).ToString().ToLowerInvariant();
            Assert.Equal(value.ToLowerInvariant(), currentValue);
        }

        [Then("JSON '(.*)'='(.*)'")]
        public void ThenEqualsTo(string key, string value)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            var currentValue = jsonHttpBody.SelectToken(key).ToString().ToLowerInvariant();
            Assert.Equal(value.ToLowerInvariant(), currentValue);
        }

        [Then("JSON '(.*)' contains '(.*)'")]
        public void ThenContains(string key, string value)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            var currentValue = (jsonHttpBody[key] as JArray).Values<string>().ToList();
            Assert.True(currentValue.Contains(value) == true);
        }

        [Then("extract JSON '(.*)', JSON exists '(.*)'")]
        public void ThenExtractJSONExists(string jsonKey, string key)
        {
            var jsonHttpBody = _scenarioContext[jsonKey] as JObject;
            Assert.NotNull(jsonHttpBody.SelectToken(key));
        }

        [Then("extract JSON '(.*)', JSON doesn't exist '(.*)'")]
        public void ThenExtractJSONDoesntExist(string jsonKey, string key)
        {
            var jsonHttpBody = _scenarioContext[jsonKey] as JObject;
            Assert.Null(jsonHttpBody.SelectToken(key));
        }

        [Then("JSON exists '(.*)'")]
        public void ThenJsonExists(string key)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            Assert.NotNull(jsonHttpBody.SelectToken(key));
        }
    }
}
