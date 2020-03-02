using CaseManagement.CMMN.Infrastructures;

namespace CaseManagement.CMMN.Domains.Role.Events
{
    public class RoleDeletedEvent : DomainEvent
    {
        public RoleDeletedEvent(string id, string aggregateId, int version) : base(id, aggregateId, version)
        {
        }
    }
}
