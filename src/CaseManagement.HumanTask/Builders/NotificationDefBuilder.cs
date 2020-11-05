using CaseManagement.HumanTask.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.HumanTask.Builders
{
    public class NotificationDefBuilder
    {
        private readonly NotificationDefinition _notification;

        public NotificationDefBuilder(string name)
        {
            _notification = new NotificationDefinition
            {
                Name = name
            };
        }

        public NotificationDefBuilder SetPriority(int priority)
        {
            _notification.Priority = priority;
            return this;
        }

        public NotificationDefBuilder AddInputOperationParameter(string name, ParameterTypes type, bool isRequired)
        {
            _notification.OperationParameters.Add(new Parameter
            {
                Name = name,
                Type = type,
                IsRequired = isRequired,
                Usage = ParameterUsages.INPUT
            });
            return this;
        }

        public NotificationDefBuilder AddOutputOperationParameter(string name, ParameterTypes type, bool isRequired)
        {
            _notification.OperationParameters.Add(new Parameter
            {
                Name = name,
                Type = type,
                IsRequired = isRequired,
                Usage = ParameterUsages.OUTPUT
            });
            return this;
        }

        #region Presentation element

        public NotificationDefBuilder AddName(string language, string value)
        {
            _notification.PresentationElements.Add(new PresentationElementDefinition
            {
                Language = language,
                Value = value,
                Usage = PresentationElementUsages.NAME
            });
            return this;
        }

        public NotificationDefBuilder AddSubject(string language, string value, string contentType)
        {
            _notification.PresentationElements.Add(new PresentationElementDefinition
            {
                Language = language,
                Value = value,
                Usage = PresentationElementUsages.SUBJECT
            });
            return this;
        }

        public NotificationDefBuilder AddDescription(string language, string value, string contentType)
        {
            _notification.PresentationElements.Add(new PresentationElementDefinition
            {
                Language = language,
                Value = value,
                ContentType = contentType,
                Usage = PresentationElementUsages.DESCRIPTION
            });
            return this;
        }

        public NotificationDefBuilder AddPresentationParameter(string name, ParameterTypes type, string expression)
        {
            _notification.PresentationParameters.Add(new PresentationParameter
            {
                Expression = expression,
                Name = name,
                Type = type
            });
            return this;
        }

        #endregion

        #region Recipient

        public NotificationDefBuilder SetRecipientGroupNames(ICollection<string> groupNames)
        {
            foreach(var groupName in groupNames)
            {
                _notification.PeopleAssignments.Add(new PeopleAssignmentDefinition
                {
                    Type = PeopleAssignmentTypes.GROUPNAMES,
                    Usage = PeopleAssignmentUsages.RECIPIENT,
                    Value = groupName
                });
            }

            return this;
        }

        public NotificationDefBuilder SetRecipientUserIdentifiers(ICollection<string> userIdentifiers)
        {
            foreach (var userIdentifier in userIdentifiers)
            {
                _notification.PeopleAssignments.Add(new PeopleAssignmentDefinition
                {
                    Type = PeopleAssignmentTypes.USERIDENTIFIERS,
                    Usage = PeopleAssignmentUsages.RECIPIENT,
                    Value = userIdentifier
                });
            }

            return this;
        }

        #endregion

        #region Business administrator

        public NotificationDefBuilder SetBusinessAdministratorUserIdentifiers(ICollection<string> userIdentifiers)
        {
            foreach (var userIdentifier in userIdentifiers)
            {
                _notification.PeopleAssignments.Add(new PeopleAssignmentDefinition
                {
                    Type = PeopleAssignmentTypes.USERIDENTIFIERS,
                    Usage = PeopleAssignmentUsages.BUSINESSADMINISTRATOR,
                    Value = userIdentifier
                });
            }

            return this;
        }

        #endregion

        public NotificationDefinition Build()
        {
            return _notification;
        }
    }
}
