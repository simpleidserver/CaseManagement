using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CaseManagement.HumanTask.Domains
{
    public class HumanTaskInstanceExpressionContext : BaseExpressionContext
    {
        private readonly Dictionary<string, string> _outputParameters;
        private readonly ICollection<HumanTaskInstanceAggregate> _subTasks;

        public HumanTaskInstanceExpressionContext(
            HumanTaskInstanceAggregate humanTaskInstance, 
            Dictionary<string, string> outputParameters,
            ICollection<HumanTaskInstanceAggregate> subTasks) : base(humanTaskInstance.InputParameters)
        {
            _outputParameters = outputParameters;
            _subTasks = subTasks;
        }

        #region Input operations

        /// <summary>
        /// Get the input parameter of the sub task.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subTaskName"></param>
        /// <returns></returns>
        public string GetInput(string key, string subTaskName)
        {
            var t = _subTasks.FirstOrDefault(_ => _.HumanTaskDefName == subTaskName);
            if (t == null)
            {
                return string.Empty;
            }

            return GetParameter(t.InputParameters, key);
        }

        public int GetIntInput(string key, string subTaskName)
        {
            var str = GetInput(key, subTaskName);
            return GetInt(str);
        }

        #endregion

        #region Output operations

        public int GetIntOutput(string key, string subTaskName)
        {
            var str = GetOutput(key, subTaskName);
            return GetInt(str);
        }

        /// <summary>
        /// Get the output parameter of the given sub task.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetOutput(string key, string subTaskName)
        {
            var t = _subTasks.FirstOrDefault(_ => _.HumanTaskDefName == subTaskName);
            if (t == null)
            {
                return string.Empty;
            }

            return GetParameter(t.OutputParameters, key);
        }

        /// <summary>
        /// Get output parameter of the current task.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetOutput(string key)
        {
            return GetParameter(_outputParameters, key);
        }

        #endregion

        private int GetInt(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            if (int.TryParse(str, out int r))
            {
                return r;
            }

            return 0;
        }
    }
}
