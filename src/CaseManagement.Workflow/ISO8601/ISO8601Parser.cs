using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace CaseManagement.Workflow.ISO8601
{
    public class ISO8601Parser
    {
        public static DateTime? ParseTime(string expression)
        {
            DateTime result;
            if (!DateTime.TryParse(expression, out result))
            {
                return null;
            }

            return result;
        }

        public static ISO8601TimeInterval ParseTimeInterval(string expression)
        {
            if (IsTimeIntervalDuration(expression))
            {
                var splitted = expression.Split('/');
                return new ISO8601TimeInterval(DateTime.Parse(splitted.First()), DateTime.Parse(splitted.Last()));
            }

            if (IsTimeIntervalStartAndDuration(expression))
            {
                var splitted = expression.Split('/');
                var startDate = DateTime.Parse(splitted.First());
                var duration = XmlConvert.ToTimeSpan(splitted.Last());
                var endDate = startDate.Add(duration);
                return new ISO8601TimeInterval(startDate, endDate);
            }

            if (IsTimeIntervalDurationAndEnd(expression))
            {
                var splitted = expression.Split('/');
                var endDate = DateTime.Parse(splitted.Last());
                var duration = XmlConvert.ToTimeSpan(splitted.First());
                var startDate = endDate.Add(-duration);
                return new ISO8601TimeInterval(startDate, endDate);
            }

            if (IsDuration(expression))
            {
                var startDate = DateTime.UtcNow;
                var endDate = startDate.Add(XmlConvert.ToTimeSpan(expression));
                return new ISO8601TimeInterval(startDate, endDate);
            }

            return null;
        }

        public static ISO8601RepeatingTimeInterval ParseRepeatingTimeInterval(string expression)
        {
            if (IsRepeatingInterval(expression))
            {
                var splitted = expression.Split('/');
                var interval = ParseTimeInterval(string.Join("/", splitted.Skip(1)));
                if (interval == null)
                {
                    return null;
                }

                var nbIterationsStr = splitted.First().Replace("R", "");
                var nbIterations = 1;
                if (!string.IsNullOrWhiteSpace(nbIterationsStr))
                {
                    nbIterations = int.Parse(nbIterationsStr);
                }

                return new ISO8601RepeatingTimeInterval(nbIterations, interval);
            }

            return null;
        }

        public static bool IsTimeIntervalDuration(string expression)
        {
            var regex = new Regex(@"^([\+-]?\d{4}(?!\d{2}\b))((-?)((0[1-9]|1[0-2])(\3([12]\d|0[1-9]|3[01]))?|W([0-4]\d|5[0-2])(-?[1-7])?|(00[1-9]|0[1-9]\d|[12]\d{2}|3([0-5]\d|6[1-6])))([T\s]((([01]\d|2[0-3])((:?)[0-5]\d)?|24\:?00)([\.,]\d+(?!:))?)?(\17[0-5]\d([\.,]\d+)?)?([zZ]|([\+-])([01]\d|2[0-3]):?([0-5]\d)?)?)?)?(\/)([\+-]?\d{4}(?!\d{2}\b))((-?)((0[1-9]|1[0-2])(\3([12]\d|0[1-9]|3[01]))?|W([0-4]\d|5[0-2])(-?[1-7])?|(00[1-9]|0[1-9]\d|[12]\d{2}|3([0-5]\d|6[1-6])))([T\s]((([01]\d|2[0-3])((:?)[0-5]\d)?|24\:?00)([\.,]\d+(?!:))?)?(\17[0-5]\d([\.,]\d+)?)?([zZ]|([\+-])([01]\d|2[0-3]):?([0-5]\d)?)?)?)?$");
            return regex.IsMatch(expression);
        }

        public static bool IsTimeIntervalStartAndDuration(string expression)
        {
            var regex = new Regex(@"^()([\+-]?\d{4}(?!\d{2}\b))((-?)((0[1-9]|1[0-2])(\4([12]\d|0[1-9]|3[01]))?|W([0-4]\d|5[0-2])(-?[1-7])?|(00[1-9]|0[1-9]\d|[12]\d{2}|3([0-5]\d|6[1-6])))([T\s]((([01]\d|2[0-3])((:?)[0-5]\d)?|24\:?00)([\.,]\d+(?!:))?)?(\18[0-5]\d([\.,]\d+)?)?([zZ]|([\+-])([01]\d|2[0-3]):?([0-5]\d)?)?)?)?(\/)P(?:\d+(?:\.\d+)?Y)?(?:\d+(?:\.\d+)?M)?(?:\d+(?:\.\d+)?W)?(?:\d+(?:\.\d+)?D)?(?:T(?:\d+(?:\.\d+)?H)?(?:\d+(?:\.\d+)?M)?(?:\d+(?:\.\d+)?S)?)?$");
            return regex.IsMatch(expression);
        }

        public static bool IsTimeIntervalDurationAndEnd(string expression)
        {
            var regex = new Regex(@"^()P(?:\d+(?:\.\d+)?Y)?(?:\d+(?:\.\d+)?M)?(?:\d+(?:\.\d+)?W)?(?:\d+(?:\.\d+)?D)?(?:T(?:\d+(?:\.\d+)?H)?(?:\d+(?:\.\d+)?M)?(?:\d+(?:\.\d+)?S)?)?\/([\+-]?\d{4}(?!\d{2}\b))((-?)((0[1-9]|1[0-2])(\4([12]\d|0[1-9]|3[01]))?|W([0-4]\d|5[0-2])(-?[1-7])?|(00[1-9]|0[1-9]\d|[12]\d{2}|3([0-5]\d|6[1-6])))([T\s]((([01]\d|2[0-3])((:?)[0-5]\d)?|24\:?00)([\.,]\d+(?!:))?)?(\18[0-5]\d([\.,]\d+)?)?([zZ]|([\+-])([01]\d|2[0-3]):?([0-5]\d)?)?)?)?$");
            return regex.IsMatch(expression);
        }

        public static bool IsDuration(string expression)
        {
            var regex = new Regex(@"^(-?)P(?=\d|T\d)(?:(\d+)Y)?(?:(\d+)M)?(?:(\d+)([DW]))?(?:T(?:(\d+)H)?(?:(\d+)M)?(?:(\d+(?:\.\d+)?)S)?)?$");
            return regex.IsMatch(expression);
        }

        public static bool IsRepeatingInterval(string expression)
        {
            var regex = new Regex(@"^(R\d*\/).*$");
            return regex.IsMatch(expression);
        }
    }
}
