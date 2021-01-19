using CaseManagement.CMMN.CasePlanInstance.Processors.FileItem;
using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Infrastructure.ExternalEvts;
using CaseManagement.Common.Processors;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Processors
{
    public class CaseFileItemInstanceProcessor : BaseCaseEltInstanceProcessor<CaseFileItemInstance>
    {
        private readonly IEnumerable<ICaseFileItemStore> _caseFileItemStores;

        public CaseFileItemInstanceProcessor(ISubscriberRepository subscriberRepository, IEnumerable<ICaseFileItemStore> caseFileItemStores) : base(subscriberRepository) 
        {
            _caseFileItemStores = caseFileItemStores;
        }

        protected override async Task Handle(CMMNExecutionContext executionContext, CaseFileItemInstance elt, CancellationToken cancellationToken)
        {
            var update = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Update, cancellationToken);
            var replace = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Replace, cancellationToken);
            var removeChild = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.RemoveChild, cancellationToken);
            var addChild = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.AddChild, cancellationToken);
            var addReference = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.AddReference, cancellationToken);
            var removeReference = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.RemoveReference, cancellationToken);
            var delete = await TrySubscribe(executionContext, elt, CMMNConstants.ExternalTransitionNames.Delete, cancellationToken);
            if (elt.State == CaseFileItemStates.Available)
            {
                var caseFileItemStore = _caseFileItemStores.FirstOrDefault(_ => _.CaseFileItemType == elt.DefinitionType);
                if (caseFileItemStore == null)
                {
                    // TODO : THROW EXCEPTION.
                }

                await caseFileItemStore.TryAddCaseFileItem(elt, executionContext.Instance, cancellationToken);
                if (update.IsCaptured)
                {
                    var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Update, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Update, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                    return;
                }

                if (replace.IsCaptured)
                {
                    var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Replace, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Replace, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                    return;
                }

                if (removeChild.IsCaptured)
                {
                    var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.RemoveChild, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.RemoveChild, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                    return;
                }

                if (addChild.IsCaptured)
                {
                    var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.AddChild, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.AddChild, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                    return;
                }

                if (addReference.IsCaptured)
                {
                    var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.AddReference, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.AddReference, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                    return;
                }

                if (removeReference.IsCaptured)
                {
                    var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.RemoveReference, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.RemoveReference, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                    return;
                }

                if (delete.IsCaptured)
                {
                    var sub = await TryReset(executionContext, elt, CMMNConstants.ExternalTransitionNames.Delete, cancellationToken);
                    executionContext.Instance.MakeTransition(elt, CMMNTransitions.Delete, incomingTokens: MergeParameters(executionContext, sub.Parameters));
                }
            }
        }
    }
}
