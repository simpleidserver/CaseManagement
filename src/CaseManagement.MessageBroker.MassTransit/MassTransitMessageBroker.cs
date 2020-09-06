using CaseManagement.CMMN.Infrastructures.Bus;
using MassTransit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.MessageBroker.MassTransit
{
    public class MassTransitMessageBroker : IMessageBroker
    {
        /*
        private readonly MessageBrokerOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public MassTransitMessageBroker(IOptions<MessageBrokerOptions> options, IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public async Task Start()
        {
            var consumerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .SelectMany(a => a.GetInterfaces())
                .Where(s => s.IsGenericType && s.GetGenericTypeDefinition() == typeof(IMessageBrokerConsumerGeneric<>));
            var queueNames = new List<string>();
            foreach (var consumerType in consumerTypes)
            {
                var obj = _serviceProvider.GetService(consumerType);
                var queueName = typeof(IMessageBrokerConsumer).GetProperty("QueueName").GetValue(obj).ToString();
                if (!queueNames.Contains(queueName))
                {
                    queueNames.Add(queueName);
                }
            }

            foreach (var queueName in queueNames)
            {
                var receivedEdp = _options.BusControl.ConnectReceiveEndpoint(queueName, (c) =>
                {
                    c.Consumer(() => new MassTransitMessageConsumer(_serviceProvider));
                });

                await receivedEdp.Ready;
            }
        }

        public async Task Queue(string queueName, object message)
        {
            var sendEdp = await _options.BusControl.GetSendEndpoint(_options.UriCallback(queueName));
            await sendEdp.Send(new MassTransitMessage(message.GetType().AssemblyQualifiedName, message));
        }

        public Task Stop()
        {
            return _options.BusControl.StopAsync();
        }

        public void Dispose()
        {
            Stop();
        }

        private class MassTransitMessage
        {
            public MassTransitMessage(string assemblyQualifiedName, object message)
            {
                AssemblyQualifiedName = assemblyQualifiedName;
                Message = message;
            }

            public string AssemblyQualifiedName { get; set; }
            public object Message { get; set; }
        }

        private class MassTransitMessageConsumer : IConsumer<MassTransitMessage>
        {
            private readonly IServiceProvider _serviceProvider;

            public MassTransitMessageConsumer(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public Task Consume(ConsumeContext<MassTransitMessage> context)
            {
                var genericType = Type.GetType(context.Message.AssemblyQualifiedName);
                var messageBrokerType = typeof(IMessageBrokerConsumerGeneric<>).MakeGenericType(genericType);
                var lst = (IEnumerable<object>)_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(messageBrokerType));
                var deserialized = JsonConvert.DeserializeObject(context.Message.Message.ToString(), genericType);
                foreach (var r in lst)
                {
                    messageBrokerType.GetMethod("Handle").Invoke(r, new[] { deserialized, CancellationToken.None });
                }

                return Task.CompletedTask;
            }
        }
        */
        public Task<T> Dequeue<T>(string queueName, CancellationToken cancellationToken) where T : class
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task Queue(string queueName, object message)
        {
            throw new NotImplementedException();
        }

        public Task Start()
        {
            throw new NotImplementedException();
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
