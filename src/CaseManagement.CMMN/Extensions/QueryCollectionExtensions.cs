using CaseManagement.CMMN.Persistence.Parameters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CaseManagement.CMMN.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static bool TryGet(this IQueryCollection queryCollection, string name, out string[] values)
        {
            values = null;
            if (queryCollection.ContainsKey(name))
            {
                var result = queryCollection[name].ToArray().Where(s => !string.IsNullOrWhiteSpace(s)).Distinct();
                if (!result.Any())
                {
                    return false;
                }

                values = result.ToArray();
                return true;
            }

            return false;
        }

        public static bool TryGet(this IQueryCollection queryCollection, string name, out int[] values)
        {
            values = null;
            string[] tmp;
            if (!queryCollection.TryGet(name, out tmp))
            {
                return false;
            }

            var result = new List<int>();
            foreach (var str in tmp)
            {
                var splitted = str.Split(',');
                foreach (var record in splitted)
                {
                    int number;
                    if (int.TryParse(record, out number))
                    {
                        result.Add(number);
                    }
                }
            }

            if (!result.Any())
            {
                return false;
            }

            values = result.ToArray();
            return true;
        }

        public static bool TryGet(this IQueryCollection queryCollection, string name, out DateTime value)
        {
            value = default(DateTime);
            if (queryCollection.ContainsKey(name))
            {
                DateTime result;
                if (DateTime.TryParse(queryCollection[name].ToArray().First(), out result))
                {
                    value = result;
                    return true;
                }

                return false;
            }

            return false;
        }

        public static bool TryGet(this IQueryCollection queryCollection, string name, out string value)
        {
            value = null;
            if (queryCollection.ContainsKey(name))
            {
                value = queryCollection[name].ToArray().First();
                return true;
            }

            return false;
        }

        public static bool TryGet(this IQueryCollection queryCollection, string name, out FindOrders order)
        {
            string orderStr;
            order = FindOrders.ASC;
            if (!queryCollection.TryGet(name, out orderStr))
            {
                return false;
            }

            FindOrders result;
            if (Enum.TryParse(orderStr.ToUpperInvariant(), out result))
            {
                order = result;
                return true;
            }

            return false;
        }

        public static bool TryGet(this IQueryCollection queryCollection, string name, out int startIndex)
        {
            startIndex = 0;
            if (queryCollection.ContainsKey(name))
            {
                return int.TryParse(queryCollection[name].First(), out startIndex);
            }

            return false;
        }
    }
}
