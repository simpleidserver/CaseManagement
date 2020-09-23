namespace CaseManagement.Common.Processors
{
    public class ExecutionResult
    {
        protected ExecutionResult(bool isNext = false, bool isBlocked = false, object outcome = null)
        {
            IsNext = isNext;
            IsBlocked = isBlocked;
            OutcomeValue = outcome;
        }

        public bool IsNext { get; private set; }
        public bool IsBlocked { get; private set; }
        public object OutcomeValue { get; private set; }

        public static ExecutionResult Next()
        {
            return new ExecutionResult(isNext: true);
        }

        public static ExecutionResult Outcome(object outcome)
        {
            return new ExecutionResult(isNext: true, outcome: outcome);
        }

        public static ExecutionResult Block()
        {
            return new ExecutionResult(isBlocked: true);
        }
    }
}
