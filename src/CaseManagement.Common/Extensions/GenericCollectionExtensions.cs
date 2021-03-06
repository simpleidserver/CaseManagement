﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace System.Collections.Generic
{
    public static class GenericCollectionExtensions
    {
        public static string GetUserNameIdentifier(this IEnumerable<KeyValuePair<string, string>> claims)
        {
            if (!claims.Any(_ => _.Key == ClaimTypes.NameIdentifier))
            {
                return string.Empty;
            }

            return claims.First(_ => _.Key == ClaimTypes.NameIdentifier).Value;
        }
    }
}
