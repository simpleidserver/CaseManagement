using CaseManagement.CMMN.CaseInstance.EventHandlers;
using CaseManagement.Workflow.Domains.Events;
using CaseManagement.Workflow.Infrastructure.EvtBus;
using Hangfire;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCMMN(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseHangfireServer();
            appBuilder.ConfigureEventBus();
            appBuilder.UseMvc();
            return appBuilder;
        }

        private static void ConfigureEventBus(this IApplicationBuilder app)
        {
            var evtBus = (IEventBus)app.ApplicationServices.GetService(typeof(IEventBus));
            evtBus.Subscribe<ProcessFlowInstanceCreatedEvent, ProcessFlowInstanceCreatedEventHandler>();
            evtBus.Subscribe<ProcessFlowInstanceLaunchedEvent, ProcessFlowInstanceLaunchedEventHandler>();
            evtBus.Subscribe<ProcessFlowInstanceFormConfirmedEvent, ProcessFlowInstanceFormConfirmedEventHandler>();
            evtBus.Subscribe<ProcessFlowElementLaunchedEvent, ProcessFlowElementLaunchedEventHandler>();
        }
    }
}
