﻿using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public class ActivateCommandHandler : BaseExternalEventNotification, IRequestHandler<ActivateCommand, bool>
    {
        public ActivateCommandHandler(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor) : base(casePlanInstanceCommandRepository, subscriberRepository, casePlanInstanceProcessor)
        {
        }

        public override string EvtName => CMMNConstants.ExternalTransitionNames.ManualStart;

        public Task<bool> Handle(ActivateCommand command, CancellationToken token)
        {
            return Execute(command.CasePlanInstanceId, command.CasePlanElementInstanceId, command.Parameters, token);
        }
    }
}