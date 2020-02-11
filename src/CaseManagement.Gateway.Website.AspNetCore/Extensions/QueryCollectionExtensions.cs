using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CaseManagement.Gateway.Website.AspNetCore.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> ToEnumerable(this IQueryCollection query)
        {
            var result = new List<KeyValuePair<string, string>>();
            foreach (var record in query)
            {
                result.Add(new KeyValuePair<string, string>(record.Key, record.Value));
            }

            return result;
        }
    }
}
