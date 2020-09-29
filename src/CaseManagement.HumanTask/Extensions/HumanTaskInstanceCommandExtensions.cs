using CaseManagement.HumanTask.Domains;
using static CaseManagement.HumanTask.HumanTaskInstance.Commands.CreateHumanTaskInstanceCommand;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    public static class HumanTaskInstanceCommandExtensions
    {
        public static TaskPeopleAssignment ToDomain(this CreateHumanTaskInstanceAssignPeople cmd)
        {
            var result = new TaskPeopleAssignment();
            result.BusinessAdministrator = cmd.BusinessAdministrator?.ToDomain();
            result.ExcludedOwner = cmd.ExcludedOwner?.ToDomain();
            result.PotentialOwner = cmd.PotentialOwner?.ToDomain();
            result.Recipient = cmd.Recipient?.ToDomain();
            result.TaskStakeHolder = cmd.TaskStakeHolder?.ToDomain();
            return result;
        }

        private static PeopleAssignment ToDomain(this AssignPeople assignPeople)
        {
            if (assignPeople.Expression != null)
            {
                return new ExpressionAssignment
                {
                    Expression = assignPeople.Expression.Expression
                };
            }

            if (assignPeople.GroupNames != null)
            {
                return new GroupNamesAssignment
                {
                    GroupNames = assignPeople.GroupNames.GroupNames
                };
            }

            if (assignPeople.LogicalPeopleGroup != null)
            {
                return new LogicalPeopleGroupAssignment
                {
                    LogicalPeopleGroup = assignPeople.LogicalPeopleGroup.LogicalPeopleGroup,
                    Arguments = assignPeople.LogicalPeopleGroup.Arguments
                };
            }

            if (assignPeople.UserIdentifiers != null)
            {
                return new UserIdentifiersAssignment
                {
                    UserIdentifiers = assignPeople.UserIdentifiers.UserIdentifiers
                };
            }

            return null;
        }
    }
}
