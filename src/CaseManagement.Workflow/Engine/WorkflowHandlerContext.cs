using CaseManagement.Workflow.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Workflow.Engine
{
    public class WorkflowHandlerContext
    {
        private readonly IProcessFlowElementProcessorFactory _processFlowElementProcessorFactory;

        public WorkflowHandlerContext(ProcessFlowInstance processFlowInstance, ProcessFlowInstanceElement currentElement, IProcessFlowElementProcessorFactory processFlowElementProcessorFactory)
        {
            ProcessFlowInstance = processFlowInstance;
            CurrentElement = currentElement;
            _processFlowElementProcessorFactory = processFlowElementProcessorFactory;
        }

        public ProcessFlowInstance ProcessFlowInstance { get; set; }   
        public ProcessFlowInstanceElement CurrentElement { get; set; }

        public void Start()
        {
            ProcessFlowInstance.StartElement(CurrentElement);
        }

        public async Task Complete(CancellationToken token)
        {
            ProcessFlowInstance.CompleteElement(CurrentElement);
            await this.ExecuteNext(token);
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                if (!ProcessFlowInstance.PreviousElements(CurrentElement.Id).All(e => e.Status == ProcessFlowInstanceElementStatus.Finished) || CurrentElement.Status == ProcessFlowInstanceElementStatus.Launched)
                {
                    return;
                }

                if (CurrentElement.Status == ProcessFlowInstanceElementStatus.Finished)
                {
                    await ExecuteNext(cancellationToken);
                    return;
                }

                var processor = _processFlowElementProcessorFactory.Build(CurrentElement);
                await processor.Handle(this, cancellationToken);
            }
            catch(Exception e)
            {
                ProcessFlowInstance.InvalidElement(CurrentElement, e.ToString());
            }
        }

        public Task ExecuteNext(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach(var next in ProcessFlowInstance.NextElements(CurrentElement.Id))
            {
                var handlerContext = new WorkflowHandlerContext(ProcessFlowInstance, next, _processFlowElementProcessorFactory);
                tasks.Add(handlerContext.Execute(cancellationToken));
            }

            return Task.WhenAll(tasks);
        }
    }
}
