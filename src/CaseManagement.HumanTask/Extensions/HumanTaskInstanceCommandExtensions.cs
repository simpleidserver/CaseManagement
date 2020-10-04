using CaseManagement.HumanTask.Domains;
using static CaseManagement.HumanTask.HumanTaskInstance.Commands.CreateHumanTaskInstanceCommand;

namespace CaseManagement.HumanTask.HumanTaskInstance.Commands
{
    public static class HumanTaskInstanceCommandExtensions
    {
        public static HumanTaskDefinitionAssignment ToDomain(this CreateHumanTaskInstanceAssignPeople cmd)
        {
            var result = new HumanTaskDefinitionAssignment();
            result.BusinessAdministrator = cmd.BusinessAdministrator?.ToDomain();
            result.ExcludedOwner = cmd.ExcludedOwner?.ToDomain();
            result.PotentialOwner = cmd.PotentialOwner?.ToDomain();
            result.Recipient = cmd.Recipient?.ToDomain();
            result.TaskStakeHolder = cmd.TaskStakeHolder?.ToDomain();
            return result;
        }

        private static PeopleAssignmentDefinition ToDomain(this AssignPeople assignPeople)
        {
            if (assignPeople.Expression != null)
            {
                return new ExpressionAssignmentDefinition
                {
                    Expression = assignPeople.Expression.Expression
                };
            }

            if (assignPeople.GroupNames != null)
            {
                return new GroupNamesAssignmentDefinition
                {
                    GroupNames = assignPeople.GroupNames.GroupNames
                };
            }

            if (assignPeople.LogicalPeopleGroup != null)
            {
                return new LogicalPeopleGroupAssignmentDefinition
                {
                    LogicalPeopleGroup = assignPeople.LogicalPeopleGroup.LogicalPeopleGroup,
                    Arguments = assignPeople.LogicalPeopleGroup.Arguments
                };
            }

            if (assignPeople.UserIdentifiers != null)
            {
                return new UserIdentifiersAssignmentDefinition
                {
                    UserIdentifiers = assignPeople.UserIdentifiers.UserIdentifiers
                };
            }

            return null;
        }
    }
}
