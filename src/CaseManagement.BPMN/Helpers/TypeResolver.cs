using System;
using System.Linq;

namespace CaseManagement.BPMN.Helpers
{
    public static class TypeResolver
    {
        public static Type ResolveType(string fullName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(_ => _.GetType(fullName) != null)
                .Select(_ => _.GetType(fullName))
                .FirstOrDefault();
        }
    }
}
