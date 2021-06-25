using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CaseManagement.BPMN.Acceptance.Tests.Steps
{
    [Binding]
    public class DataExtractSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public DataExtractSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When("extract JSON from body")]
        public async Task WhenExtractFromBody()
        {
            var httpResponseMessage = _scenarioContext["httpResponseMessage"] as HttpResponseMessage;
            var json = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            _scenarioContext.Set(JsonConvert.DeserializeObject<JObject>(json), "jsonHttpBody");
        }

        [When("extract JSON from body into '(.*)'")]
        public async Task WhenExtractFromBodyIntoKey(string key)
        {
            var httpResponseMessage = _scenarioContext["httpResponseMessage"] as HttpResponseMessage;
            var json = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            _scenarioContext.Set(JsonConvert.DeserializeObject<JObject>(json), key);
        }

        [When("extract '(.*)' from JSON body into '(.*)'")]
        public void WhenExtractJSONKeyFromBody(string selector, string key)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            var val = jsonHttpBody.SelectToken(selector);
            if (val != null)
            {
               _scenarioContext.Set(val.ToString(), key);
            }
        }
    }
}
