using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
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
        private const int MS = 400;
        private static Semaphore _obj = new Semaphore(initialCount: 1, maximumCount: 1);
        private readonly ScenarioContext _scenarioContext;
        private static CustomWebApplicationFactory<FakeStartup> _factory;
        private static HttpClient _client;

        public SharedSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _obj.WaitOne();
            _factory = new CustomWebApplicationFactory<FakeStartup>(c =>
            {
                c.AddSingleton(_scenarioContext);
            });
            _client = _factory.CreateClient();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _obj.Release();
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
            var httpResponseMessage = await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);
            _scenarioContext.Set(httpResponseMessage, "httpResponseMessage");
        }

        [When("poll '(.*)', until '(.*)'='(.*)'")]
        public async Task WhenPollHTTPGETRequest(string url, string key, string value)
        {
            url = Parse(url);
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            var httpResponseMessage = await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);
            var json = await httpResponseMessage.Content.ReadAsStringAsync();
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            if (jObj == null)
            {
                Thread.Sleep(MS);
                await WhenPollHTTPGETRequest(url, key, value);
                return;
            }

            var token = jObj.SelectToken(key);
            if (token == null || token.ToString() != value)
            {
                Thread.Sleep(MS);
                await WhenPollHTTPGETRequest(url, key, value);
                return;
            }

            _scenarioContext.Set(httpResponseMessage, "httpResponseMessage");
        }
        
        [When("execute HTTP POST JSON request '(.*)'")]
        public async Task WhenExecuteHTTPPostJSONRequest(string url, Table table)
        {
            var jObj = new JObject();
            foreach (var record in table.Rows)
            {
                var key = record["Key"];
                var value = Parse(record["Value"]);
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
            var httpResponseMessage = await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);
            _scenarioContext.Set(httpResponseMessage, "httpResponseMessage");
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

        [When("wait '(.*)' seconds")]
        public void WhenWaitSeconds(string seconds)
        {
            Thread.Sleep(int.Parse(seconds));
        }

        [When("add a file into the folder '(.*)'")]
        public void ThenAddFileIntoFolder(string key)
        {
            var currentValue = Parse(key);
            File.WriteAllText(Path.Combine(currentValue, $"{Guid.NewGuid().ToString()}.txt"), Guid.NewGuid().ToString());
        }

        [When("authenticate as '(.*)'")]
        public void WhenAuthenticate(string key)
        {
            _scenarioContext.Add("userId", key);
        }

        [Then("HTTP status code equals to '(.*)'")]
        public void ThenCheckHttpStatusCode(int code)
        {
            var httpResponseMessage = _scenarioContext["httpResponseMessage"] as HttpResponseMessage;
            Assert.Equal(code, (int)httpResponseMessage.StatusCode);
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

        [Then("extract JSON '(.*)', JSON exists '(.*)'")]
        public void ThenExtractJSONExists(string jsonKey, string key)
        {
            var jsonHttpBody = _scenarioContext[jsonKey] as JObject;
            Assert.NotNull(jsonHttpBody.SelectToken(key));
        }

        [Then("JSON exists '(.*)'")]
        public void ThenJsonExists(string key)
        {
            var jsonHttpBody = _scenarioContext["jsonHttpBody"] as JObject;
            Assert.NotNull(jsonHttpBody.SelectToken(key));
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
