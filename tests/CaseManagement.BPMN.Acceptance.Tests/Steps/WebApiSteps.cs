using CaseManagement.BPMN.Acceptance.Tests.Middlewares;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace CaseManagement.BPMN.Acceptance.Tests.Steps
{
    [Binding]
    public class WebApiSteps
    {
        private class FakeHttpClientFactory : CaseManagement.Common.Factories.IHttpClientFactory
        {
            private readonly Func<HttpClient> _callback;

            public FakeHttpClientFactory(Func<HttpClient> callback)
            {
                _callback = callback;
            }

            public HttpClient Build()
            {
                return _callback();
            }
        }

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
                        c.AddSingleton<CaseManagement.Common.Factories.IHttpClientFactory>(new FakeHttpClientFactory(() => _factory.CreateClient()));
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

        [When("wait '(.*)'")]
        public void WaisMS(int s)
        {
            Thread.Sleep(s * 1000);
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
