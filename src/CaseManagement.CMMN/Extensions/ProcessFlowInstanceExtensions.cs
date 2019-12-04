using CaseManagement.CMMN.Domains;
using System;
using System.Linq;

namespace CaseManagement.Workflow.Domains
{
    public static class ProcessFlowInstanceExtensions
    {
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

        public static CMMNPlanItem GetPlanItem(this ProcessFlowInstance pf, string id)
        {
            return pf.Elements.FirstOrDefault(e => e.Id == id) as CMMNPlanItem;
        }
    }
}