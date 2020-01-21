using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace CaseManagement.CMMN.AspNetCore.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetNameIdentifier(this Controller controller)
        {
            return (controller.User.Identity as ClaimsIdentity).Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}