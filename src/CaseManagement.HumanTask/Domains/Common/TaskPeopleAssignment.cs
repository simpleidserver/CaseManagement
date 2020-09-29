using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.HumanTask.Domains
{
    public class TaskPeopleAssignment : ICloneable
    {
        /// <summary>
        /// Potential owners of a task are persons who receive the task so that they can claim and complete it.
        /// A potential owner becomes the actual owner of a task by explicitly claiming it.
        /// </summary>
        public PeopleAssignment PotentialOwner { get; set; }
        /// <summary>
        /// Excluded owners are are people who cannot become an actual or potential owner and thus they cannot reserve or start the task.
        /// </summary>
        public PeopleAssignment ExcludedOwner { get; set; }
        /// <summary>
        /// Is the person who creates the task instance.
        /// </summary>
        public PeopleAssignment TaskInitiator { get; set; }
        /// <summary>
        /// The task stakeholders are the people ultimately responsible for the oversight and outcome of the task instance
        /// </summary>
        public PeopleAssignment TaskStakeHolder { get; set; }
        /// <summary>
        /// Business administrators play the same role as task stakeholders but at task definition level.
        /// </summary>
        public PeopleAssignment BusinessAdministrator { get; set; }
        /// <summary>
        /// Notification recipients are persons who receive the notification.
        /// </summary>
        public PeopleAssignment Recipient { get; set; }

        public object Clone()
        {
            return new TaskPeopleAssignment
            {
                PotentialOwner = (PeopleAssignment)PotentialOwner?.Clone(),
                ExcludedOwner = (PeopleAssignment)ExcludedOwner?.Clone(),
                TaskInitiator = (PeopleAssignment)TaskInitiator?.Clone(),
                TaskStakeHolder = (PeopleAssignment)TaskStakeHolder?.Clone(),
                BusinessAdministrator = (PeopleAssignment)BusinessAdministrator?.Clone(),
                Recipient = (PeopleAssignment)Recipient?.Clone()
            };
        }
    }

    #region Different assignment strategies

    public abstract class PeopleAssignment : ICloneable
    {
        public abstract PeopleAssignmentTypes Type { get; }

        public abstract object Clone();
    }

    public class LogicalPeopleGroupAssignment : PeopleAssignment
    {
        public override PeopleAssignmentTypes Type => PeopleAssignmentTypes.LOGICALPEOPLEGROUP;
        public string LogicalPeopleGroup { get; set; }
        public Dictionary<string, string> Arguments { get; set; }

        public override object Clone()
        {
            return new LogicalPeopleGroupAssignment
            {
                LogicalPeopleGroup = LogicalPeopleGroup,
                Arguments = Arguments.ToDictionary(c => c.Key, c => c.Value)
            };
        }
    }

    public class UserIdentifiersAssignment : PeopleAssignment
    {
        public UserIdentifiersAssignment()
        {
            UserIdentifiers = new List<string>();
        }

        public override PeopleAssignmentTypes Type => PeopleAssignmentTypes.USERIDENTFIERS;
        public ICollection<string> UserIdentifiers { get; set; }

        public override object Clone()
        {
            return new UserIdentifiersAssignment
            {
                UserIdentifiers = UserIdentifiers.ToList()
            };
        }
    }

    public class GroupNamesAssignment : PeopleAssignment
    {
        public GroupNamesAssignment()
        {
            GroupNames = new List<string>();
        }

        public override PeopleAssignmentTypes Type => PeopleAssignmentTypes.GROUPNAMES;
        public ICollection<string> GroupNames { get; set; }

        public override object Clone()
        {
            return new GroupNamesAssignment
            {
                GroupNames = GroupNames.ToList()
            };
        }
    }

    public class ExpressionAssignment : PeopleAssignment
    {
        public override PeopleAssignmentTypes Type => PeopleAssignmentTypes.EXPRESSION;
        public string Expression { get; set; }

        public override object Clone()
        {
            return new ExpressionAssignment
            {
                Expression = Expression
            };
        }
    }

    #endregion
}