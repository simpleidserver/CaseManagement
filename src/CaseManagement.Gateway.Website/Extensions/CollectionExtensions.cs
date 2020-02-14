using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.Gateway.Website.Extensions
{
    public static class CollectionExtensions
    {
        public static void TryReplace(this ICollection<KeyValuePair<string, string>> queries, string name, string value)
        {
            if (queries.Any(k => k.Key == name))
            {
                queries.Remove(queries.First(k => k.Key == name));
            }

            queries.Add(new KeyValuePair<string, string>(name, value));
        }
    }
}
