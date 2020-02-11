using CaseManagement.Gateway.Website.Extensions;
using SimpleIdServer.Jwt.Jws;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SimpleIdServer.Jwt.Constants;

namespace CaseManagement.Gateway.Website.Token
{
    public class TokenStore : ITokenStore
    {
        private class StoredAccessToken
        {
            public IEnumerable<string> Scopes { get; set; }
            public string AccessToken { get; set; }
            public DateTime ExpirationDateTime { get; set; }

            public bool Exist(IEnumerable<string> scopes)
            {
                if (scopes.All(s => Scopes.Contains(s)))
                {
                    return true;
                }

                return false;
            }
        }
        private readonly ITokenService _tokenService;
        private readonly List<StoredAccessToken> _tokens;
        private readonly IJwsGenerator _jwsGenerator;

        public TokenStore(ITokenService tokenService)
        {
            _tokenService = tokenService;
            _tokens = new List<StoredAccessToken>();
            _jwsGenerator = new JwsGenerator(null);
        }

        public async Task<string> GetValidToken(IEnumerable<string> scopes)
        {
            var storedToken = _tokens.FirstOrDefault(t => t.Exist(scopes));
            if (storedToken != null)
            {
                if (DateTime.UtcNow <= storedToken.ExpirationDateTime)
                {
                    return storedToken.AccessToken;
                }

                _tokens.Remove(storedToken);
            }

            var token = await _tokenService.GetAccessToken(scopes);
            var jwsPayload = _jwsGenerator.ExtractPayload(token);
            var expirationTime = jwsPayload.GetDoubleClaim(OAuthClaims.ExpirationTime).ConvertFromUnixTimestamp();
            _tokens.Add(new StoredAccessToken { AccessToken = token, ExpirationDateTime = expirationTime, Scopes = scopes });
            return token;
        }
    }
}
