using System.Linq;
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
    }
}