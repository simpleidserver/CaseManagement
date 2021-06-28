using CaseManagement.CMMN.Domains;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CaseManagement.CMMN.Extensions
{
    public static class CaseEltInstanceExtensions
    {
        public static string GetDefinitionType(this CaseEltInstance caseEltInstance)
        {
            return caseEltInstance.GetProperty("definition_type");
        }

        public static void UpdateDefinitionType(this CaseEltInstance caseEltInstance, string value)
        {
            caseEltInstance.UpdateProperty("definition_type", value);
        }

        public static string GetPerformerRef(this CaseEltInstance caseEltInstance)
        {
            return caseEltInstance.GetProperty("performer_ref");
        }

        public static void UpdatePerformerRef(this CaseEltInstance caseEltInstance, string value)
        {
            caseEltInstance.UpdateProperty("performer_ref", value);
        }

        public static string GetImplementation(this CaseEltInstance caseEltInstance)
        {
            return caseEltInstance.GetProperty("implementation");
        }

        public static void UpdateImplementation(this CaseEltInstance caseEltInstance, string value)
        {
            caseEltInstance.UpdateProperty("implementation", value);
        }

        public static string GetFormId(this CaseEltInstance caseEltInstance)
        {
            return caseEltInstance.GetProperty("form_id");
        }

        public static void UpdateFormId(this CaseEltInstance caseEltInstance, string value)
        {
            caseEltInstance.UpdateProperty("form_id", value);
        }

        public static Dictionary<string, string> GetInputParameters(this CaseEltInstance caseEltInstance)
        {
            var record = caseEltInstance.GetProperty("input_parameters");
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(record);
        }

        public static void UpdateInputParameters(this CaseEltInstance caseEltInstance, Dictionary<string, string> inputParameters)
        {
            caseEltInstance.UpdateProperty("input_parameters", JsonConvert.SerializeObject(inputParameters));
        }

        public static ICollection<ParameterMapping> GetMappings(this CaseEltInstance caseEltInstance)
        {
            return JsonConvert.DeserializeObject<ICollection<ParameterMapping>>(caseEltInstance.GetProperty("mappings"));
        }

        public static void UpdateMappings(this CaseEltInstance caseEltInstance, ICollection<ParameterMapping> mappings)
        {
            caseEltInstance.UpdateProperty("mappings", JsonConvert.SerializeObject(mappings));
        }

        public static string GetProcessRef(this CaseEltInstance caseEltInstance)
        {
            return caseEltInstance.GetProperty("process_ref");
        }

        public static void UpdateProcessRef(this CaseEltInstance caseEltInstance, string processRef)
        {
            caseEltInstance.UpdateProperty("process_ref", processRef);
        }

        public static string GetSourceRef(this CaseEltInstance caseEltInstance)
        {
            return caseEltInstance.GetProperty("source_ref");
        }

        public static void UpdateSourceRef(this CaseEltInstance caseEltInstance, string sourceRef)
        {
            caseEltInstance.UpdateProperty("source_ref", sourceRef);
        }

        public static CMMNExpression GetParameterRefExpression(this CaseEltInstance caseEltInstance)
        {
            return JsonConvert.DeserializeObject<CMMNExpression>(caseEltInstance.GetProperty("process_ref_expression"));
        }

        public static void UpdateParameterRefExpression(this CaseEltInstance caseEltInstance, CMMNExpression expression)
        {
            caseEltInstance.UpdateProperty("process_ref_expression", JsonConvert.SerializeObject(expression));
        }

        public static CMMNExpression GetTimerExpression(this CaseEltInstance caseEltInstance)
        {
            return JsonConvert.DeserializeObject<CMMNExpression>(caseEltInstance.GetProperty("timer_expression"));
        }

        public static void UpdateTimerExpression(this CaseEltInstance caseEltInstance, CMMNExpression expression)
        {
            caseEltInstance.UpdateProperty("timer_expression", JsonConvert.SerializeObject(expression));
        }
    }
}
