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

        public NotificationDefBuilder SetPriority(string priority)
        {
            _notification.Priority = priority;
            return this;
        }

        public NotificationDefBuilder SetOperation(Action<OperationBuilder> callback)
        {
            var builder = new OperationBuilder();
            callback(builder);
            _notification.Operation = builder.Build();
            return this;
        }

        #region Presentation element

        public NotificationDefBuilder AddName(string language, string value)
        {
            _notification.PresentationElement.Names.Add(new Text { Language = language, Value = value });
            return this;
        }

        public NotificationDefBuilder AddSubject(string language, string value, string contentType)
        {
            _notification.PresentationElement.Subjects.Add(new Text { Language = language, Value = value });
            return this;
        }

        public NotificationDefBuilder AddDescription(string language, string value, string contentType)
        {
            _notification.PresentationElement.Descriptions.Add(new Description { Language = language, Value = value, ContentType = contentType });
            return this;
        }

        public NotificationDefBuilder AddPresentationParameter(string name, ParameterTypes type, string expression)
        {
            _notification.PresentationElement.PresentationParameters.Add(new PresentationParameter
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
            _notification.PeopleAssignment.Recipient = new GroupNamesAssignmentDefinition
            {
                GroupNames = groupNames
            };
            return this;
        }

        public NotificationDefBuilder SetRecipientUserIdentifiers(ICollection<string> userIdentifiers)
        {
            _notification.PeopleAssignment.Recipient = new UserIdentifiersAssignmentDefinition
            {
                UserIdentifiers = userIdentifiers
            };
            return this;
        }

        #endregion

        #region Business administrator

        public NotificationDefBuilder SetBusinessAdministratorUserIdentifiers(ICollection<string> userIdentifiers)
        {
            _notification.PeopleAssignment.BusinessAdministrator = new UserIdentifiersAssignmentDefinition
            {
                UserIdentifiers = userIdentifiers
            };
            return this;
        }

        #endregion

        public NotificationDefinition Build()
        {
            return _notification;
        }
    }
}
