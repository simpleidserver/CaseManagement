using CaseManagement.CMMN.CasePlanInstance.Exceptions;
using CaseManagement.CMMN.CasePlanInstance.Processors;
using CaseManagement.CMMN.Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.CMMN.CasePlanInstance.Commands.Handlers
{
    public abstract class BaseExternalEventNotification
    {
        private readonly ICasePlanInstanceCommandRepository _casePlanInstanceCommandRepository;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly ICasePlanInstanceProcessor _casePlanInstanceProcessor;

        public BaseExternalEventNotification(
            ICasePlanInstanceCommandRepository casePlanInstanceCommandRepository,
            ISubscriberRepository subscriberRepository,
            ICasePlanInstanceProcessor casePlanInstanceProcessor)
        {
            _casePlanInstanceCommandRepository = casePlanInstanceCommandRepository;
            _subscriberRepository = subscriberRepository;
            _casePlanInstanceProcessor = casePlanInstanceProcessor;
        }

        public abstract string EvtName { get; }

        public async Task<bool> Execute(string casePlanInstanceId, string casePlanElementInstanceId, Dictionary<string, string> parameters, CancellationToken token)
        {
            var casePlanInstance = await _casePlanInstanceCommandRepository.Get(casePlanInstanceId, token);
            if (casePlanInstance == null)
            {
                throw new UnknownCasePlanInstanceException(casePlanInstanceId);
            }

            if (!string.IsNullOrWhiteSpace(casePlanElementInstanceId))
            {
                var elt = casePlanInstance.GetChild(casePlanElementInstanceId);
                if (elt == null)
                {
                    throw new UnknownCasePlanElementInstanceException(casePlanInstanceId, casePlanElementInstanceId);
                }
            }

            var subscriber = await _subscriberRepository.Get(casePlanInstanceId, casePlanElementInstanceId, EvtName, token);
            if (subscriber == null)
            {
                throw new InvalidOperationException("subscriber doesn't exist");
            }

            subscriber.IsCaptured = true;
            subscriber.Parameters = parameters;
            await _subscriberRepository.Update(subscriber, token);
            await _casePlanInstanceProcessor.Execute(casePlanInstance, token);
            await _casePlanInstanceCommandRepository.Update(casePlanInstance, token);
            await _casePlanInstanceCommandRepository.SaveChanges(token);
            return true;
        }
    }
}
