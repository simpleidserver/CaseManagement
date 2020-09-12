using CaseManagement.CMMN.CasePlanInstance.Processors.FileItem;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CaseFileItemInstanceProcessor : BaseProcessor<CaseFileItemInstance>
    {
        private readonly IEnumerable<ICaseFileItemStore> _caseFileItemStores;

        public CaseFileItemInstanceProcessor(ISubscriberRepository subscriberRepository, IEnumerable<ICaseFileItemStore> caseFileItemStores) : base(subscriberRepository) 
        {
            _caseFileItemStores = caseFileItemStores;
        }

        public override async Task Execute(CMMNExecutionContext executionContext, CaseFileItemInstance elt, CancellationToken cancellationToken)
        {
            var update = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Update, cancellationToken);
            var replace = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Replace, cancellationToken);
            var removeChild = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.RemoveChild, cancellationToken);
            var addChild = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.AddChild, cancellationToken);
            var addReference = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.AddReference, cancellationToken);
            var removeReference = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.RemoveReference, cancellationToken);
            var delete = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Delete, cancellationToken);
            if (elt.State == null)
            {
                executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Create);
            }

            if (elt.State == CaseFileItemStates.Available)
            {
                var caseFileItemStore = _caseFileItemStores.FirstOrDefault(_ => _.CaseFileItemType == elt.DefinitionType);
                if (caseFileItemStore == null)
                {
                    // TODO : THROW EXCEPTION.
                }

                await caseFileItemStore.TryAddCaseFileItem(elt, executionContext.CasePlanInstance, cancellationToken);
                if (update.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Update);
                    await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Update, cancellationToken);
                    return;
                }

                if (replace.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Replace);
                    await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Replace, cancellationToken);
                    return;
                }

                if (removeChild.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.RemoveChild);
                    await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.RemoveChild, cancellationToken);
                    return;
                }

                if (addChild.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.AddChild);
                    await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.AddChild, cancellationToken);
                    return;
                }

                if (addReference.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.AddReference);
                    await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.AddReference, cancellationToken);
                    return;
                }

                if (removeReference.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.RemoveReference);
                    await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.RemoveReference, cancellationToken);
                    return;
                }

                if (delete.IsCaptured)
                {
                    executionContext.CasePlanInstance.MakeTransition(elt, CMMNTransitions.Delete);
                }
            }
        }
    }
}
