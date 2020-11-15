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

        public HumanTaskDefBuilder AddInputOperationParameter(string name, ParameterTypes type, bool isRequired)
        {
            _humanTaskDef.OperationParameters.Add(new Parameter
             {
                 Name = name,
                 Type = type,
                 IsRequired = isRequired,
                 Usage = ParameterUsages.INPUT
             });
            return this;
        }

        public HumanTaskDefBuilder AddOutputOperationParameter(string name, ParameterTypes type, bool isRequired)
        {
            _humanTaskDef.OperationParameters.Add(new Parameter
            {
                Name = name,
                Type = type,
                IsRequired = isRequired,
                Usage = ParameterUsages.OUTPUT
            });
            return this;
        }

        public HumanTaskDefBuilder AddCallbackOperation(string url)
        {
            _humanTaskDef.CallbackOperations.Add(new CallbackOperation
            {
                Url = url
            });
            return this;
        }

        #region Deadlines

        public HumanTaskDefBuilder AddStartDeadLine(string name, Action<DeadLineBuilder> callback)  
        {
            var deadLineBuilder = new DeadLineBuilder(name, DeadlineUsages.START);
            callback(deadLineBuilder);
            _humanTaskDef.DeadLines.Add(deadLineBuilder.Build());
            return this;
        }

        public HumanTaskDefBuilder AddCompletionDeadLine(string name, Action<DeadLineBuilder> callback)
        {
            var deadLineBuilder = new DeadLineBuilder(name, DeadlineUsages.COMPLETION);
            callback(deadLineBuilder);
            _humanTaskDef.DeadLines.Add(deadLineBuilder.Build());
            return this;
        }

        #endregion

        #region Presentation element

        public HumanTaskDefBuilder AddName(string language, string value)
        {
            _humanTaskDef.PresentationElements.Add(new PresentationElementDefinition
            {
                Language = language,
                Value = value,
                Usage = PresentationElementUsages.NAME
            });
            return this;
        }

        public HumanTaskDefBuilder AddSubject(string language, string value, string contentType)
        {
            _humanTaskDef.PresentationElements.Add(new PresentationElementDefinition
            {
                Language = language,
                Value = value,
                Usage = PresentationElementUsages.SUBJECT
            });
            return this;
        }

        public HumanTaskDefBuilder AddDescription(string language, string value, string contentType)
        {
            _humanTaskDef.PresentationElements.Add(new PresentationElementDefinition
            {
                Language = language,
                Value = value,
                ContentType = contentType,
                Usage = PresentationElementUsages.DESCRIPTION
            });
            return this;
        }

        public HumanTaskDefBuilder AddPresentationParameter(string name, ParameterTypes type, string expression)
        {
            _humanTaskDef.PresentationParameters.Add(new PresentationParameter
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
            foreach (var userIdentifier in userIdentifiers)
            {
                _humanTaskDef.PeopleAssignments.Add(new PeopleAssignmentDefinition
                {
                    Usage = PeopleAssignmentUsages.TASKINITIATOR,
                    Type = PeopleAssignmentTypes.USERIDENTIFIERS,
                    Value = userIdentifier
                });
            }

            return this;
        }

        #endregion

        #region Potential owner

        public HumanTaskDefBuilder SetPotentialOwnerGroupNames(ICollection<string> groupNames)
        {
            foreach (var groupName in groupNames)
            {
                _humanTaskDef.PeopleAssignments.Add(new PeopleAssignmentDefinition
                {
                    Usage = PeopleAssignmentUsages.POTENTIALOWNER,
                    Type = PeopleAssignmentTypes.GROUPNAMES,
                    Value = groupName
                });
            }

            return this;
        }

        public HumanTaskDefBuilder SetPotentialOwnerUserIdentifiers(ICollection<string> userIdentifiers)
        {
            foreach (var userIdentifier in userIdentifiers)
            {
                _humanTaskDef.PeopleAssignments.Add(new PeopleAssignmentDefinition
                {
                    Usage = PeopleAssignmentUsages.POTENTIALOWNER,
                    Type = PeopleAssignmentTypes.USERIDENTIFIERS,
                    Value = userIdentifier
                });
            }

            return this;
        }

        #endregion

        #region Business administrator

        public HumanTaskDefBuilder SetBusinessAdministratorUserIdentifiers(ICollection<string> userIdentifiers)
        {
            foreach(var userIdentifier in userIdentifiers)
            {
                _humanTaskDef.PeopleAssignments.Add(new PeopleAssignmentDefinition
                {
                    Usage = PeopleAssignmentUsages.BUSINESSADMINISTRATOR,
                    Type = PeopleAssignmentTypes.USERIDENTIFIERS,
                    Value = userIdentifier
                });
            }

            return this;
        }

        #endregion

        #region Completion behavior

        public HumanTaskDefBuilder SetManualCompletion(Action<CompletionBehaviorBuilder> callback)
        {
            var builder = new CompletionBehaviorBuilder();
            callback(builder);
            _humanTaskDef.CompletionAction = CompletionBehaviors.MANUAL;
            _humanTaskDef.Completions = builder.Build();
            return this;
        }

        public HumanTaskDefBuilder SetAutomaticCompletion(Action<CompletionBehaviorBuilder> callback)
        {
            var builder = new CompletionBehaviorBuilder();
            callback(builder);
            _humanTaskDef.CompletionAction = CompletionBehaviors.AUTOMATIC;
            _humanTaskDef.Completions = builder.Build();
            return this;
        }

        #endregion

        #region Composition

        public HumanTaskDefBuilder SetParallelComposition(InstantiationPatterns instantiationPattern, Action<CompositionBuilder> callback)
        {
            var builder = new CompositionBuilder();
            callback(builder);
            _humanTaskDef.Type = CompositionTypes.PARALLEL;
            _humanTaskDef.InstantiationPattern = instantiationPattern;
            _humanTaskDef.SubTasks = builder.Build();
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
