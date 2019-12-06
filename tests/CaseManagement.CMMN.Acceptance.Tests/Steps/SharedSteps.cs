using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace CaseManagement.CMMN.Acceptance.Tests.Steps
{
    [Binding]
    public class SharedSteps
    {
        // private static Semaphore _obj = new Semaphore(initialCount: 1, maximumCount: 1);
        private readonly ScenarioContext _scenarioContext;
        private static CustomWebApplicationFactory<FakeStartup> _factory;

        public SharedSteps(ScenarioContext scenarioContext)
        {
            // _obj.WaitOne();
            _scenarioContext = scenarioContext;
            if (_factory == null)
            {
                _factory = new CustomWebApplicationFactory<FakeStartup>();
            }
        }

        [When("execute HTTP GET request '(.*)'")]
        public async Task WhenExecuteHTTPGETRequest(string url)
        {
            url = Parse(url);
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            var httpResponseMessage = await _factory.CreateClient().SendAsync(httpRequestMessage).ConfigureAwait(false);
            _scenarioContext.Set(httpResponseMessage, "httpResponseMessage");
        }
        
        [When("execute HTTP POST JSON request '(.*)'")]
        public async Task WhenExecuteHTTPPostJSONRequest(string url, Table table)
        {
            var jObj = new JObject();
            foreach (var record in table.Rows)
            {
                var key = record["Key"];
                var value = record["Value"];
                try
                {
                    jObj.Add(key, JToken.Parse(value));
                }
                catch
                {
                    jObj.Add(key, value.ToString());
                }
            }

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Parse(url)),
                Content = new StringContent(jObj.ToString(), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await _factory.CreateClient().SendAsync(httpRequestMessage).ConfigureAwait(false);
            _scenarioContext.Set(httpResponseMessage, "httpResponseMessage");
        }

        [When("extract JSON from body")]
        public async Task GivenExtractFromBody()
        {
            var httpResponseMessage = _scenarioContext["httpResponseMessage"] as HttpResponseMessage;
            var json = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            _scenarioContext.Set(JsonConvert.DeserializeObject<JObject>(json), "jsonHttpBody");
        }

        [When("extract '(.*)' from JSON body")]
        public void WhenExtractJSONKeyFromBody(string key)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            var val = jsonHttpBody.SelectToken(key);
            if (val != null)
            {
                _scenarioContext.Set(val.ToString(), key);
            }
        }

        [When("wait '(.*)' seconds")]
        public void WhenWaitSeconds(string seconds)
        {
            Thread.Sleep(int.Parse(seconds) * 1000);
        }

        [Then("HTTP status code equals to '(.*)'")]
        public void ThenCheckHttpStatusCode(int code)
        {
            // _obj.Release();
            var httpResponseMessage = _scenarioContext["httpResponseMessage"] as HttpResponseMessage;
            Assert.Equal(code, (int)httpResponseMessage.StatusCode);
        }

        [Then("JSON contains '(.*)'")]
        public void ThenExists(string key)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            Assert.True(jsonHttpBody.ContainsKey(key) == true);
        }

        [Then("JSON '(.*)'='(.*)'")]
        public void ThenEqualsTo(string key, string value)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            var currentValue = jsonHttpBody.SelectToken(key).ToString().ToLowerInvariant();
            Assert.Equal(value.ToLowerInvariant(), currentValue);
        }

        private string Parse(string val)
        {
            var regularExpression = new Regex(@"\$([a-zA-Z]|_)*\$");
            var result = regularExpression.Replace(val, (m) =>
            {
                if (string.IsNullOrWhiteSpace(m.Value))
                {
                    return string.Empty;
                }

                return _scenarioContext.Get<string>(m.Value.TrimStart('$').TrimEnd('$'));
            });
            return result;
        }
    }
}
