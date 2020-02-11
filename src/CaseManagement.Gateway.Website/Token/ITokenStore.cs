using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaseManagement.Gateway.Website.Token
{
    public interface ITokenStore
    {
        Task<string> GetValidToken(IEnumerable<string> scopes);
    }
}
