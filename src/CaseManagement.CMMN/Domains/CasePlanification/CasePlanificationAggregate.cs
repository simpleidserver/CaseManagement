using System;

namespace CaseManagement.CMMN.Domains
{
    public class CasePlanificationAggregate : ICloneable
    {
        public string CaseInstanceId { get; set; }
        public string CaseName { get; set; }
        public string CaseDescription { get; set; }
        public string CaseElementId { get; set; }
        public string CaseElementName { get; set; }
        public string UserRole { get; set; }
        public DateTime CreateDateTime { get; set; }

        public object Clone()
        {
            return new CasePlanificationAggregate
            {
                CaseInstanceId = CaseInstanceId,
                CaseName = CaseName,
                CaseDescription = CaseDescription,
                CaseElementId = CaseElementId,
                CaseElementName = CaseElementName,
                UserRole = UserRole,
                CreateDateTime = CreateDateTime
            };
        }

        public override bool Equals(object obj)
        {
            var target = obj as CasePlanificationAggregate;
            if (target == null)
            {
                return false;
            }

            return this.GetHashCode() == target.GetHashCode();
        }

        public override int GetHashCode()
        {
            return CaseInstanceId.GetHashCode() + CaseElementId.GetHashCode();
        }
    }
}
