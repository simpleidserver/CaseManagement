using SimpleIdServer.Jwt.Jws;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Token
{
    public interface ITokenService
    {
        Task<string> GetAccessToken(IEnumerable<string> scopes);
    }
}
