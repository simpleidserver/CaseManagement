namespace CaseManagement.BPMN.Persistence.EF.Models
{
    public enum MessageTokenDirections
    {
        INCOMING = 0,
        OUTGOING = 1
    }

    public class MessageTokenModel
    {
        public long Id { get; set; }
        public MessageTokenDirections Direction { get; set; }
        public string Name { get; set; }
        public string SerializedContent { get; set; }
    }
}
