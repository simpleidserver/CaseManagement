using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace CaseManagement.CMMN.AspNet.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetNameIdentifier(this ApiController controller)
        {
            return (controller.User.Identity as ClaimsIdentity).Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        public static JObject ToError(this ApiController controller, ICollection<KeyValuePair<string, string>> errors, HttpStatusCode statusCode, HttpRequestMessage request)
        {
            var result = new JObject
            {
                { "instance", request.RequestUri.AbsoluteUri },
                { "status", (int)statusCode },
                { "detail",  "Please refer to the errors property for additional details."}
            };
            var error = new JObject();
            foreach (var kvp in errors.GroupBy(e => e.Key))
            {
                error.Add(kvp.Key, new JArray(kvp.Select(s => s.Value)));
            }

            result.Add("errors", error);
            return result;
        }
    }
}