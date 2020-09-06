
namespace CaseManagement.CMMN.Domains
{
    public class HumanTaskElementInstance : BaseTaskOrStageElementInstance
    {
        /// <summary>
        /// The performer of the humanTask (role [0...1]).
        /// </summary>
        public string PerformerRef { get; set; }
        public string FormId { get; set; }
    }
}
