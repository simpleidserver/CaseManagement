using System.Linq;
using System.Security.Claims;

namespace System.Collections.Generic
{
    public static class GenericCollectionExtensions
    {
        public static bool IsEmpty(this KeyValuePair<string, string> kvp)
        {
            if (kvp.Equals(default(KeyValuePair<string, string>)) || string.IsNullOrWhiteSpace(kvp.Value))
            {
                return true;
            }

            return false;
        }

        public static string GetUserNameIdentifier(this IEnumerable<KeyValuePair<string, string>> claims)
        {
            if (!claims.Any(_ => _.Key == ClaimTypes.NameIdentifier))
            {
                return string.Empty;
            }

            return claims.First(_ => _.Key == ClaimTypes.NameIdentifier).Value;
        }

        public static ICollection<string> GetGroupNames(this IEnumerable<KeyValuePair<string, string>> claims)
        {
            return claims.Where(_ => _.Key == ClaimTypes.Role).Select(_ => _.Value).ToList();
        }
    }
}
