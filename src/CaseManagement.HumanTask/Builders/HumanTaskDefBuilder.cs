﻿using CaseManagement.HumanTask.Domains;
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

        #region Deadlines

        public HumanTaskDefBuilder AddStartDeadLine(string name, Action<DeadLineBuilder> callback)  
        {
            var deadLineBuilder = new DeadLineBuilder(name);
            callback(deadLineBuilder);
            _humanTaskDef.DeadLines.StartDeadLines.Add(deadLineBuilder.Build());
            return this;
        }

        public HumanTaskDefBuilder AddCompletionDeadLine(string name, Action<DeadLineBuilder> callback)
        {
            var deadLineBuilder = new DeadLineBuilder(name);
            callback(deadLineBuilder);
            _humanTaskDef.DeadLines.CompletionDeadLines.Add(deadLineBuilder.Build());
            return this;
        }

        #endregion

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
            _humanTaskDef.PeopleAssignment.TaskInitiator = new UserIdentifiersAssignmentDefinition
            {
                UserIdentifiers = userIdentifiers
            };
            return this;
        }

        #endregion

        #region Potential owner

        public HumanTaskDefBuilder SetPotentialOwnerGroupNames(ICollection<string> groupNames)
        {
            _humanTaskDef.PeopleAssignment.PotentialOwner = new GroupNamesAssignmentDefinition
            {
                GroupNames = groupNames
            };
            return this;
        }

        public HumanTaskDefBuilder SetPotentialOwnerUserIdentifiers(ICollection<string> userIdentifiers)
        {
            _humanTaskDef.PeopleAssignment.PotentialOwner = new UserIdentifiersAssignmentDefinition
            {
                UserIdentifiers = userIdentifiers
            };
            return this;
        }

        #endregion

        #region Business administrator

        public HumanTaskDefBuilder SetBusinessAdministratorUserIdentifiers(ICollection<string> userIdentifiers)
        {
            _humanTaskDef.PeopleAssignment.BusinessAdministrator = new UserIdentifiersAssignmentDefinition
            {
                UserIdentifiers = userIdentifiers
            };
            return this;
        }

        #endregion

        #region Completion behavior

        public HumanTaskDefBuilder SetManualCompletion(Action<CompletionBehaviorBuilder> callback)
        {
            var builder = new CompletionBehaviorBuilder(CompletionBehaviors.MANUAL);
            callback(builder);
            _humanTaskDef.CompletionBehavior = builder.Build();
            return this;
        }

        public HumanTaskDefBuilder SetAutomaticCompletion(Action<CompletionBehaviorBuilder> callback)
        {
            var builder = new CompletionBehaviorBuilder(CompletionBehaviors.AUTOMATIC);
            callback(builder);
            _humanTaskDef.CompletionBehavior = builder.Build();
            return this;
        }

        #endregion

        #region Composition

        public HumanTaskDefBuilder SetParallelComposition(InstantiationPatterns instantiationPattern, Action<CompositionBuilder> callback)
        {
            var builder = new CompositionBuilder(CompositionTypes.PARALLEL, instantiationPattern);
            callback(builder);
            _humanTaskDef.Composition = builder.Build();
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