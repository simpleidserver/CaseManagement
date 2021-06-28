using CaseManagement.Common.Domains;

namespace CaseManagement.CMMN.Domains
{
    public class CaseElementOccuredEvent : DomainEvent
    {
        public CaseElementOccuredEvent(string id, string aggregateId, int version, string eltId) : base(id, aggregateId, version)
        {
            EltId = eltId;
        }

        public string EltId { get; set; }
    }
}
