using CaseManagement.BPMN.Domains;
using CaseManagement.BPMN.Exceptions;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.BPMN.Host.Delegates
{
    public class UpdateUserPasswordDelegate : IDelegateHandler
    {
        private const string ActivityName = "Activity_12xhvyl";

        public async Task<ICollection<MessageToken>> Execute(ICollection<MessageToken> incoming, DelegateConfigurationAggregate delegateConfiguration, CancellationToken cancellationToken)
        {
            var user = incoming.FirstOrDefault(i => i.Name == "userMessage");
            if (user == null)
            {
                throw new BPMNProcessorException("userMessage must be passed in the request");
            }

            var userId = user.GetProperty("userId");
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BPMNProcessorException("userId is not passed in the request");
            }

            var messageToken = incoming.FirstOrDefault(m => m.Name == ActivityName);
            if (messageToken == null)
            {
                throw new BPMNProcessorException($"incoming token '{ActivityName}' doesn't exist");
            }

            var password = messageToken.GetProperty("password");
            var parameter = UpdateUserPasswordParameter.Create(delegateConfiguration);
            using (var httpClient = new HttpClient())
            {
                var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = parameter.TokenUrl,
                    ClientId = parameter.ClientId,
                    ClientSecret = parameter.ClientSecret,
                    Scope = parameter.Scope
                }, cancellationToken);
                if (tokenResponse.IsError)
                {
                    throw new BPMNProcessorException(tokenResponse.Error);
                }

                var obj = new JObject
                {
                    { "password", password }
                };
                var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    Content = content,
                    RequestUri = new Uri($"{parameter.UserUrl}/{userId}/password")
                };
                request.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");
                var httpResponse = await httpClient.SendAsync(request, cancellationToken);
                httpResponse.EnsureSuccessStatusCode();
            }

            return incoming;
        }

        private class UpdateUserPasswordParameter
        {
            public string TokenUrl { get; set; }
            public string UserUrl { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string Scope { get; set; } 

            public static UpdateUserPasswordParameter Create(DelegateConfigurationAggregate delegateConfiguration)
            {
                return new UpdateUserPasswordParameter
                {
                    ClientId = delegateConfiguration.GetValue("clientId"),
                    ClientSecret = delegateConfiguration.GetValue("clientSecret"),
                    TokenUrl = delegateConfiguration.GetValue("tokenUrl"),
                    UserUrl = delegateConfiguration.GetValue("userUrl"),
                    Scope = delegateConfiguration.GetValue("scope")
                };
            }
        }
    }
}
