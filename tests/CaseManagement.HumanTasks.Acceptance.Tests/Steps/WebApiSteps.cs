using CaseManagement.HumanTasks.Acceptance.Tests.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CaseManagement.HumanTasks.Acceptance.Tests.Steps
{
    [Binding]
    public class WebApiSteps
    {
        private const int MS = 400;
        private readonly ScenarioContext _scenarioContext;
        private static object _obj = new object();
        private static CustomWebApplicationFactory<FakeStartup> _factory;
        private static HttpClient _client;
        private static IScenarioContextProvider _scenarioContextProvider;
        private static EventWaitHandle Evt = new EventWaitHandle(true, EventResetMode.AutoReset);

        public WebApiSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            lock(_obj)
            {
                Evt.WaitOne();
                if (_factory == null)
                {
                    _scenarioContextProvider = new ScenarioContextProvider();
                    _factory = new CustomWebApplicationFactory<FakeStartup>(c =>
                    {
                        c.AddSingleton(_scenarioContextProvider);
                    });
                    _client = _factory.CreateClient();
                }

                _scenarioContextProvider.SetContext(_scenarioContext);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Evt.Set();
        }

        [When("add '(.*)' seconds into '(.*)'")]
        public void WhenAddSeconds(int nbSeconds, string key)
        {
            var date = DateTime.UtcNow.AddSeconds(nbSeconds);
            _scenarioContext.Set(date.ToString(), key);
        }

        [When("authenticate")]
        public void WhenAuthenticate(Table table)
        {
            var claims = new List<Claim>();
            foreach(var record in table.Rows)
            {
                var key = record["Key"];
                var value = Parse(record["Value"]);
                claims.Add(new Claim(key, value));
            }

            _scenarioContext.Set(claims, "claims");
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

        [When("execute HTTP GET request '(.*)'")]
        public async Task WhenExecuteHTTPGETRequest(string url, Table table)
        {
            url = Parse(url);
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            foreach(var record in table.Rows)
            {
                var key = record["Key"];
                var value = Parse(record["Value"]);
                httpRequestMessage.Headers.Add(key, value);
            }

            var httpResponseMessage = await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);
            _scenarioContext.Set(httpResponseMessage, "httpResponseMessage");
        }

        [When("execute HTTP DELETE request '(.*)'")]
        public async Task WhenExecuteHTTPDELETERequest(string url)
        {
            url = Parse(url);
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
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

        [When("poll '(.*)', until '(.*)' doesn't exist")]
        public async Task WhenPollDoesntExist(string url, string key)
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
                await WhenPollDoesntExist(url, key);
                return;
            }

            var token = jObj.SelectToken(key);
            if (token != null)
            {
                Thread.Sleep(MS);
                await WhenPollDoesntExist(url, key);
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

        [When("execute HTTP PUT JSON request '(.*)'")]
        public async Task WhenExecuteHTTPPutJSONRequest(string url, Table table)
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
                Method = HttpMethod.Put,
                RequestUri = new Uri(Parse(url)),
                Content = new StringContent(jObj.ToString(), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await _client.SendAsync(httpRequestMessage).ConfigureAwait(false);
            _scenarioContext.Set(httpResponseMessage, "httpResponseMessage");
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
