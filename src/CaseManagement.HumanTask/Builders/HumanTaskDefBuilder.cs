using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Builders
{
    public class HumanTaskDefBuilder
    {
        private HumanTaskDefinitionAggregate _humanTaskDef;

        private HumanTaskDefBuilder(string name)
        {
            _humanTaskDef = new HumanTaskDefinitionAggregate
            {
                Name = name
            };
        }

        public static HumanTaskDefBuilder New(string name)
        {
            return new HumanTaskDefBuilder(name);
        }

        public HumanTaskDefBuilder SetActualOwnerRequired(bool actualOwnerRequired)
        {
            _humanTaskDef.ActualOwnerRequired = actualOwnerRequired;
            return this;
        }

        public HumanTaskDefBuilder SetPriority(int priority)
        {
            _humanTaskDef.Priority = priority;
            return this;
        }

        public HumanTaskDefBuilder SetOperation(Action<OperationBuilder> callback)
        {
            var builder = new OperationBuilder();
            callback(builder);
            _humanTaskDef.Operation = builder.Build();
            return this;
        }

        #region Presentation element

        public HumanTaskDefBuilder AddName(string language, string value)
        {
            _humanTaskDef.PresentationElement.Names.Add(new Text { Language = language, Value = value });
            return this;
        }

        public HumanTaskDefBuilder AddSubject(string language, string value, string contentType)
        {
            _humanTaskDef.PresentationElement.Subjects.Add(new Text { Language = language, Value = value });
            return this;
        }

        public HumanTaskDefBuilder AddDescription(string language, string value, string contentType)
        {
            _humanTaskDef.PresentationElement.Descriptions.Add(new Description { Language = language, Value = value, ContentType = contentType });
            return this;
        }

        public HumanTaskDefBuilder AddPresentationParameter(string name, ParameterTypes type, string expression)
        {
            _humanTaskDef.PresentationElement.PresentationParameters.Add(new PresentationParameter
            {
                Expression = expression,
                Name = name,
                Type = type
            });
            return this;
        }

        #endregion

        #region Task initiator

        public HumanTaskDefBuilder SetTaskInitiatorUserIdentifiers(ICollection<string> userIdentifiers)
        {
            _humanTaskDef.PeopleAssignment.TaskInitiator = new UserIdentifiersAssignment
            {
                UserIdentifiers = userIdentifiers
            };
            return this;
        }

        #endregion

        #region Potential owner

        public HumanTaskDefBuilder SetPotentialOwnerGroupNames(ICollection<string> groupNames)
        {
            _humanTaskDef.PeopleAssignment.PotentialOwner = new GroupNamesAssignment
            {
                GroupNames = groupNames
            };
            return this;
        }

        public HumanTaskDefBuilder SetPotentialOwnerUserIdentifiers(ICollection<string> userIdentifiers)
        {
            _humanTaskDef.PeopleAssignment.PotentialOwner = new UserIdentifiersAssignment
            {
                UserIdentifiers = userIdentifiers
            };
            return this;
        }

        #endregion

        #region Business administrator

        public HumanTaskDefBuilder SetBusinessAdministratorUserIdentifiers(ICollection<string> userIdentifiers)
        {
            _humanTaskDef.PeopleAssignment.BusinessAdministrator = new UserIdentifiersAssignment
            {
                UserIdentifiers = userIdentifiers
            };
            return this;
        }

        #endregion

        public HumanTaskDefBuilder SetOutcome(string outcome)
        {
            _humanTaskDef.Outcome = outcome;
            return this;
        }

        public HumanTaskDefBuilder SetSearchBy(string searchBy)
        {
            _humanTaskDef.SearchBy = searchBy;
            return this;
        }

        public HumanTaskDefinitionAggregate Build()
        {
            return _humanTaskDef;
        }
    }
}
