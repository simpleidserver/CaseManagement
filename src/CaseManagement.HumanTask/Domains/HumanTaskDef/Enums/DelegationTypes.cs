namespace CaseManagement.HumanTask.Domains
{
    public enum DelegationTypes
    {
        /// <summary>
        /// It is allowed to delegate the task to anybody.
        /// </summary>
        ANYBODY = 0,
        /// <summary>
        /// It is allowed to delegate the task to potential owners previously selected.
        /// </summary>
        POTENTIALOWNERS = 1,
        /// <summary>
        /// It is allowed to delegate the task to other people, e.g. authorized owners. 
        /// The element <from> is used to determine the people to whom the task can be delegated.
        /// </summary>
        OTHER = 2,
        /// <summary>
        /// It is not allowed to delegate the task.
        /// </summary>
        NOBODY = 3
    }
}
