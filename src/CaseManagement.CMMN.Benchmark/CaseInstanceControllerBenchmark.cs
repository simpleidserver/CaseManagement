using BenchmarkDotNet.Attributes;
using CaseManagement.CMMN.CasePlanInstance.Commands;
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
        private const int MS = 400;
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
            var httpResult = await _client.GetAsync("http://localhost/case-plans/search?case_plan_id=CaseWithOneTask");
            var searchResult = JsonConvert.DeserializeObject<JObject>(await httpResult.Content.ReadAsStringAsync());
            var caseDefinitionId = searchResult.SelectToken("content[0].id").ToString();
            var createCaseInstance = new CreateCaseInstanceCommand
            {
                CasePlanId = caseDefinitionId
            };
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/case-plan-instances"),
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
            var httpResult = await _client.GetAsync("http://localhost/case-plans/search?case_plan_id=CaseWithOneTask");
            var searchResult = JsonConvert.DeserializeObject<JObject>(await httpResult.Content.ReadAsStringAsync());
            var caseDefinitionId = searchResult.SelectToken("content[0].id").ToString();
            var createCaseInstance = new CreateCaseInstanceCommand
            {
                CasePlanId = caseDefinitionId
            };
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/case-plan-instances"),
                Content = new StringContent(JsonConvert.SerializeObject(createCaseInstance), Encoding.UTF8, "application/json")
            };
            httpResult = await _client.SendAsync(httpRequestMessage);
            var json = await httpResult.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<JObject>(json)["id"].ToString();
            await _client.GetAsync($"http://localhost/case-plan-instances/{id}/launch");
            await PollCaseInstanceCompleted(id);
        }

        public async Task PollCaseInstanceCreated(string id)
        {
            var httpResult = await _client.GetAsync($"http://localhost/case-plan-instances/{id}");
            if (!httpResult.IsSuccessStatusCode)
            {
                Thread.Sleep(MS);
                await PollCaseInstanceCreated(id);
                return;
            }
        }

        public async Task PollCaseInstanceCompleted(string id)
        {
            var httpResult = await _client.GetAsync($"http://localhost/case-plan-instances/{id}");
            if (!httpResult.IsSuccessStatusCode)
            {
                Thread.Sleep(MS);
                await PollCaseInstanceCompleted(id);
                return;
            }

            var json = await httpResult.Content.ReadAsStringAsync();
            var jObj = JsonConvert.DeserializeObject<JObject>(json);
            if (!jObj.ContainsKey("state"))
            {
                Thread.Sleep(MS);
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