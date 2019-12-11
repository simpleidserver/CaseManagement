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
        private readonly List<IWorkflowSubProcess> _subProcesses;

        public WorkflowHandlerContext(ProcessFlowInstance processFlowInstance, ProcessFlowInstanceElement currentElement, IProcessFlowElementProcessorFactory processFlowElementProcessorFactory)
        {
            ProcessFlowInstance = processFlowInstance;
            CurrentElement = currentElement;
            _processFlowElementProcessorFactory = processFlowElementProcessorFactory;
            _subProcesses = new List<IWorkflowSubProcess>();
        }

        public ProcessFlowInstance ProcessFlowInstance { get; set; }   
        public ProcessFlowInstanceElement CurrentElement { get; set; }
        public IProcessFlowElementProcessorFactory Factory => _processFlowElementProcessorFactory;
        public List<IWorkflowSubProcess> SubProcesses => _subProcesses;

        public void Start(CancellationToken token)
        {
            ProcessFlowInstance.StartElement(CurrentElement);
            token.ThrowIfCancellationRequested();
        }

        public Task StartSubProcess(IWorkflowSubProcess subProcess, CancellationToken token)
        {
            lock(_subProcesses)
            {
                _subProcesses.Add(subProcess);
            }

            return subProcess.Start(this, token);
        }

        public async Task Complete(CancellationToken token)
        {
            ProcessFlowInstance.CompleteElement(CurrentElement);
            await this.ExecuteNext(token);
        }

        public async Task Execute(CancellationToken cancellationToken)
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

            try
            {
                var processor = _processFlowElementProcessorFactory.Build(CurrentElement);
                await processor.Handle(this, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                ProcessFlowInstance.CancelElement(CurrentElement);
            }
            catch (Exception e)
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
