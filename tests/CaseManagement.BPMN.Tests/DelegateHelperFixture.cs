using CaseManagement.BPMN.Domains;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xunit;

namespace CaseManagement.BPMN.Tests
{
    public class DelegateHelperFixture
    {
        [Fact]
        public void Parse_Parameter()
        {
            // ARRANGE
            var message = "Please update the password by clicking on the website {{configuration.Get('humanTaskUrl')}}/humantasks/{{messages.Get('humanTaskCreated', 'humanTaskInstance.fileId')}}/instances/{{messages.Get('humanTaskCreated', 'humanTaskInstance.id')}}?auth=email";
            var incomingTokens = new List<MessageToken>
            {
                MessageToken.NewMessage(Guid.NewGuid().ToString(), "humanTaskCreated", new JObject
                {
                    { "humanTaskInstance", new JObject
                    {
                        { "id", "id" },
                        { "fileId", "fileId" }
                    }}
                }.ToString())
            };
            var configuration = new DelegateConfigurationAggregate();
            configuration.AddRecord("humanTaskUrl", "http://localhost:4200");

            // ACT
            var result = DelegateHelper.Parse(configuration, incomingTokens, message);

            // ASSERT
            Assert.Equal("Please update the password by clicking on the website http://localhost:4200/humantasks/fileId/instances/id?auth=email", result);
        }
    }
}
