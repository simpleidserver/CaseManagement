using CaseManagement.HumanTask.Domains;

namespace CaseManagement.HumanTask.Builders
{
    public class OperationBuilder
    {
        private Operation _operation;

        internal OperationBuilder()
        {
            _operation = new Operation();
        }

        public OperationBuilder AddParameter(string name, ParameterTypes type, bool isRequired = false)
        {
            _operation.Parameters.Add(new Parameter
            {
                Name = name,
                Type = type,
                IsRequired = isRequired
            });
            return this;
        }

        internal Operation Build()
        {
            return _operation;
        }
    }
}
