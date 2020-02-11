using MassTransit;
using System;

namespace CaseManagement.MessageBroker.MassTransit
{
    public class MessageBrokerOptions
    {
        public MessageBrokerOptions()
        {
            BusControl = Bus.Factory.CreateUsingInMemory(cfg => { });
            UriCallback = (queueName) => new Uri($"loopback://localhost/{queueName}", UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Get or set the bus control.
        /// </summary>
        public IBusControl BusControl { get; set; }
        /// <summary>
        /// Callback is used to return an endpoint address.
        /// </summary>
        public Func<string, Uri> UriCallback { get; set; }
    }
}
