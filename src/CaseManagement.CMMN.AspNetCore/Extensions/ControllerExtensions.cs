using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace CaseManagement.CMMN.AspNetCore.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetNameIdentifier(this Controller controller)
        {
            return (controller.User.Identity as ClaimsIdentity).Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        public static ActionResult ToError(this Controller controller, ICollection<KeyValuePair<string, string>> errors, HttpStatusCode statusCode, HttpRequest request)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Instance = request.Path,
                Status = (int)statusCode,
                Detail = "Please refer to the errors property for additional details."
            };
            foreach (var kvp in errors.GroupBy(e => e.Key))
            {
                problemDetails.Errors.Add(kvp.Key, kvp.Select(s => s.Value).ToArray());
            }

            return new BadRequestObjectResult(problemDetails)
            {
                StatusCode = (int)statusCode
            };
        }
    }
}