using System;
using System.Security.Cryptography;
using System.Text;

namespace CaseManagement.CMMN.Domains
{
    [Serializable]
    public class CaseFileItemInstance : BaseCaseEltInstance
    {
        #region Properties

        public CaseFileItemStates? State { get; set; }
        public string DefinitionType { get; set; }
        public override CasePlanElementInstanceTypes Type { get => CasePlanElementInstanceTypes.FILEITEM; }

        #endregion

        public override object Clone()
        {
            var result = new CaseFileItemInstance
            {
                DefinitionType = DefinitionType,
                State = State
            };
            FeedCaseEltInstance(result);
            return result;
        }

        protected override void UpdateTransition(CMMNTransitions transition, DateTime executionDateTime)
        {
            var newState = GetCaseFileItemState(State, transition);
            if (newState != null)
            {
                State = newState;
            }
        }

        public static string BuildId(string casePlanInstanceId, string eltId)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{casePlanInstanceId}{eltId}"));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}