using BenchmarkDotNet.Attributes;
using CaseManagement.CMMN.CaseInstance.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.Benchmark
{
    [InProcess]
    [MemoryDiagnoser]
    [RPlotExporter]
    public class CaseInstanceControllerBenchmark
    {
        private HttpClient _client;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var factory = new CustomWebApplicationFactory<Startup>();
            _client = factory.CreateClient();
        }

        [Benchmark]
        public async Task CreateCaseWithOneTask()
        {
            var httpResult = await _client.GetAsync("http://localhost/case-definitions/.search?cmmn_definition=CaseWithOneTask.cmmn");
            var searchResult = JsonConvert.DeserializeObject<JObject>(await httpResult.Content.ReadAsStringAsync());
            var caseDefinitionId = searchResult.SelectToken("content[0].id").ToString();
            var createCaseInstance = new CreateCaseInstanceCommand
            {
                CaseDefinitionId = caseDefinitionId
            };
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/case-instances"),
                Content = new StringContent(JsonConvert.SerializeObject(createCaseInstance), Encoding.UTF8, "application/json")
            };
            httpResult = await _client.SendAsync(httpRequestMessage);
            var json = await httpResult.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<JObject>(json)["id"].ToString();
            await PollCaseInstanceCreated(id);
        }

        [Benchmark]
        public async Task LaunchCaseWithOneTask()
        {
            var httpResult = await _client.GetAsync("http://localhost/case-definitions/.search?cmmn_definition=CaseWithOneTask.cmmn");
            var searchResult = JsonConvert.DeserializeObject<JObject>(await httpResult.Content.ReadAsStringAsync());
            var caseDefinitionId = searchResult.SelectToken("content[0].id").ToString();
            var createCaseInstance = new CreateCaseInstanceCommand
            {
                CaseDefinitionId = caseDefinitionId
            };
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/case-instances"),
                Content = new StringContent(JsonConvert.SerializeObject(createCaseInstance), Encoding.UTF8, "application/json")
            };
            httpResult = await _client.SendAsync(httpRequestMessage);
            var json = await httpResult.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<JObject>(json)["id"].ToString();
            await _client.GetAsync($"http://localhost/case-instances/{id}/launch");
            await PollCaseInstanceCompleted(id);
        }

        /*
        [Benchmark]
        public async Task LaunchEventListener2Seconds()
        {
            var cmd = new CreateCaseInstanceCommand
            {
                CaseDefinitionId = "caseWithTimerEventListener",
                CasesId = "Case_0d1ujq8"
            };
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/case-instances"),
                Content = new StringContent(JsonConvert.SerializeObject(cmd), Encoding.UTF8, "application/json")
            };
            var httpResult = await _client.SendAsync(httpRequestMessage);
            var json = await httpResult.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<JObject>(json)["id"].ToString();
            await _client.GetAsync($"http://localhost/case-instances/{id}/launch");
            await PollCaseInstanceCompleted(id);
        }
        */

        public async Task PollCaseInstanceCreated(string id)
        {
            var httpResult = await _client.GetAsync($"http://localhost/case-instances/{id}");
            if (!httpResult.IsSuccessStatusCode)
            {
                await PollCaseInstanceCreated(id);
                return;
            }
        }

        public async Task PollCaseInstanceCompleted(string id)
        {
            var httpResult = await _client.GetAsync($"http://localhost/case-instances/{id}");
            if (!httpResult.IsSuccessStatusCode)
            {
                Thread.Sleep(2);
                await PollCaseInstanceCompleted(id);
                return;
            }

            var json = await httpResult.Content.ReadAsStringAsync();
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            if (!jObj.ContainsKey("state"))
            {
                Thread.Sleep(2);
                await PollCaseInstanceCompleted(id);
                return;
            }

            var status = jObj["state"].ToString();
            if (status != "Completed")
            {
                Thread.Sleep(2);
                await PollCaseInstanceCompleted(id);
                return;
            }
        }
    }
}