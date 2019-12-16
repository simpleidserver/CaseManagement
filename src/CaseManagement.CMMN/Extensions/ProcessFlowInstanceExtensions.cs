using CaseManagement.CMMN.Domains;
using CaseManagement.CMMN.Domains.CaseInstance.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public static class ProcessFlowInstanceExtensions
    {
        public static void CreateCaseFileItem(this ProcessFlowInstance processFlowInstance, CMMNCaseFileItem caseFileItem, Dictionary<string, string> metadata)
        {
            var evt = new CMMNCaseFileItemCreatedEvent(Guid.NewGuid().ToString(), processFlowInstance.Id, processFlowInstance.Version + 1, caseFileItem.Id, metadata);
            processFlowInstance.Handle(evt);
            processFlowInstance.DomainEvents.Add(evt);
        }

        public static void CreateManualStart(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            var evt = new CMMNManualStartCreated(Guid.NewGuid().ToString(), processFlowInstance.Id, processFlowInstance.Version + 1, planItem.Id);
            processFlowInstance.Handle(evt);
            processFlowInstance.DomainEvents.Add(evt);
        }

        public static void AddChild(this ProcessFlowInstance processFlowInstance, CMMNCaseFileItem caseFileItem)
        {
            processFlowInstance.RaiseEvent(caseFileItem.Id, CMMNCaseFileItemTransitions.AddChild);
        }

        public static void RemoveChild(this ProcessFlowInstance processFlowInstance, CMMNCaseFileItem caseFileItem)
        {
            processFlowInstance.RaiseEvent(caseFileItem.Id, CMMNCaseFileItemTransitions.RemoveChild);
        }

        public static void Delete(this ProcessFlowInstance processFlowInstance, CMMNCaseFileItem caseFileItem)
        {
            processFlowInstance.RaiseEvent(caseFileItem.Id, CMMNCaseFileItemTransitions.Delete);
        }

        public static void AddReference(this ProcessFlowInstance processFlowInstance, CMMNCaseFileItem caseFileItem)
        {
            processFlowInstance.RaiseEvent(caseFileItem.Id, CMMNCaseFileItemTransitions.AddReference);
        }

        public static void RemoveReference(this ProcessFlowInstance processFlowInstance, CMMNCaseFileItem caseFileItem)
        {
            processFlowInstance.RaiseEvent(caseFileItem.Id, CMMNCaseFileItemTransitions.RemoveReference);
        }

        public static void Update(this ProcessFlowInstance processFlowInstance, CMMNCaseFileItem caseFileItem)
        {
            processFlowInstance.RaiseEvent(caseFileItem.Id, CMMNCaseFileItemTransitions.Update);
        }

        public static void CreatePlanItem(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            processFlowInstance.RaiseEvent(planItem.Id, CMMNPlanItemTransitions.Create);
        }

        public static void CreatePlanItem(this ProcessFlowInstance processFlowInstance, string eltId)
        {
            processFlowInstance.RaiseEvent(eltId, CMMNPlanItemTransitions.Create);
        }

        public static void OccurPlanItem(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            processFlowInstance.RaiseEvent(planItem.Id, CMMNPlanItemTransitions.Occur);
        }

        public static void OccurPlanItem(this ProcessFlowInstance processFlowInstance, string eltId)
        {
            processFlowInstance.RaiseEvent(eltId, CMMNPlanItemTransitions.Occur);
        }

        public static void CompletePlanItem(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            processFlowInstance.RaiseEvent(planItem.Id, CMMNPlanItemTransitions.Complete);
        }

        public static void CompletePlanItem(this ProcessFlowInstance processFlowInstance, string eltId)
        {
            processFlowInstance.RaiseEvent(eltId, CMMNPlanItemTransitions.Complete);
        }

        public static void StartPlanItem(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            processFlowInstance.RaiseEvent(planItem.Id, CMMNPlanItemTransitions.Start);
        }

        public static void StartPlanItem(this ProcessFlowInstance processFlowInstance, string eltId)
        {
            processFlowInstance.RaiseEvent(eltId, CMMNPlanItemTransitions.Start);
        }

        public static void EnablePlanItem(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            processFlowInstance.RaiseEvent(planItem.Id, CMMNPlanItemTransitions.Enable);
        }

        public static void EnablePlanItem(this ProcessFlowInstance processFlowInstance, string eltId)
        {
            processFlowInstance.RaiseEvent(eltId, CMMNPlanItemTransitions.Enable);
        }

        public static void TerminatePlanItem(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            processFlowInstance.RaiseEvent(planItem.Id, CMMNPlanItemTransitions.Terminate);
        }

        public static void TerminatePlanItem(this ProcessFlowInstance processFlowInstance, string eltId)
        {
            processFlowInstance.RaiseEvent(eltId, CMMNPlanItemTransitions.Terminate);
        }

        public static void DisablePlanItem(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            processFlowInstance.RaiseEvent(planItem.Id, CMMNPlanItemTransitions.Disable);
        }

        public static void DisablePlanItem(this ProcessFlowInstance processFlowInstance, string eltId)
        {
            processFlowInstance.RaiseEvent(eltId, CMMNPlanItemTransitions.Disable);
        }

        public static void ManuallyStartPlanItem(this ProcessFlowInstance processFlowInstance, CMMNPlanItem planItem)
        {
            processFlowInstance.RaiseEvent(planItem.Id, CMMNPlanItemTransitions.ManualStart);
        }

        public static void ManuallyStartPlanItem(this ProcessFlowInstance processFlowInstance, string eltId)
        {
            processFlowInstance.RaiseEvent(eltId, CMMNPlanItemTransitions.ManualStart);
        }

        public static void RaiseEvent(this ProcessFlowInstance processFlowInstance, string eltId, CMMNPlanItemTransitions transition)
        {
            var name = Enum.GetName(typeof(CMMNPlanItemTransitions), transition);
            processFlowInstance.RaiseEvent(eltId, name);
        }

        public static void RaiseEvent(this ProcessFlowInstance processFlowInstance, string eltId, CMMNCaseFileItemTransitions transition)
        {
            var name = Enum.GetName(typeof(CMMNCaseFileItemTransitions), transition);
            processFlowInstance.RaiseEvent(eltId, name);
        }

        public static CMMNPlanItem GetPlanItem(this ProcessFlowInstance pf, string id)
        {
            return pf.Elements.FirstOrDefault(e => e.Id == id) as CMMNPlanItem;
        }

        public static CMMNCaseFileItem GetCaseFileItem(this ProcessFlowInstance pf, string id)
        {
            return pf.Elements.FirstOrDefault(e => e.Id == id) as CMMNCaseFileItem;
        }
    }
}