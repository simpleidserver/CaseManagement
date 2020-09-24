namespace CaseManagement.BPMN.Domains
{
    public enum GatewayDirections
    {
        /// <summary>
        /// There are no constraints.
        /// </summary>
        UNSPECIFIED = 0,
        /// <summary>
        /// This Gateway MAY have multiple incoming Sequence flows, but must have no more than one outgoing sequence flow.
        /// </summary>
        CONVERGING = 1,
        /// <summary>
        /// This Gateway MAY have multiple outgoing sequence sequence flows but must have no more than one incoming sequence flow.
        /// </summary>
        DIVERGING = 2,
        /// <summary>
        /// This gateway contains multiple outgoing and multiple incoming sequence flows.
        /// </summary>
        MIXED = 3
    }
}
