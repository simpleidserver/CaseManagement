using System.Collections.Generic;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static IEnumerable<KeyValuePair<string, string>> GetClaims(this ClaimsPrincipal claims)
        {
            var result = new List<KeyValuePair<string, string>>();
            foreach(var claim in claims.Claims)
            {
                result.Add(new KeyValuePair<string, string>(claim.Type, claim.Value));
            }

            return result;
        }
    }
}
