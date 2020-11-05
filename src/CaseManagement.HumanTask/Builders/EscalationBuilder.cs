using CaseManagement.HumanTask.Domains;
using System;

namespace CaseManagement.HumanTask.Builders
{
    public class DeadLineBuilder
    {
        private readonly HumanTaskDefinitionDeadLine _deadLine;

        public DeadLineBuilder(string name, DeadlineUsages usage)
        {
            _deadLine = new HumanTaskDefinitionDeadLine
            {
                Name = name,
                Usage = usage
            };
        }

        public DeadLineBuilder SetForExpression(string forExpression)
        {
            _deadLine.For = forExpression;
            return this;
        }

        public DeadLineBuilder SetUntilExpression(string untilExpression)
        {
            _deadLine.Until = untilExpression;
            return this;
        }

        public DeadLineBuilder AddEscalation(Action<EscalationBuilder> callback)
        {
            var builder = new EscalationBuilder();
            callback(builder);
            _deadLine.Escalations.Add(builder.Build());
            return this;
        }

        public HumanTaskDefinitionDeadLine Build()
        {
            return _deadLine;
        }
    }

    public class EscalationBuilder
    {
        private readonly Escalation _escalation;

        public EscalationBuilder()
        {
            _escalation = new Escalation();
        }

        public EscalationBuilder SetCondition(string condition)
        {
            _escalation.Condition = condition;
            return this;
        }

        public EscalationBuilder AddToPart(string name, string expression)
        {
            _escalation.ToParts.Add(new ToPart
            {
                Name = name,
                Expression = expression
            });
            return this;
        }

        public EscalationBuilder SetNotification(string name, Action<NotificationDefBuilder> callback)
        {
            var builder = new NotificationDefBuilder(name);
            callback(builder);
            _escalation.Notification = builder.Build();
            return this;
        }

        public Escalation Build()
        {
            return _escalation;
        }
    }
}
