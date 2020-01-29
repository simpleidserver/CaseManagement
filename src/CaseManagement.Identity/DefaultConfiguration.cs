// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using SimpleIdServer.OAuth.Domains;
using SimpleIdServer.OAuth.Helpers;
using SimpleIdServer.OpenID;
using SimpleIdServer.OpenID.Domains;
using System;
using System.Collections.Generic;

namespace CaseManagement.Identity
{
    public class DefaultConfiguration
    {
        private static OpenIdScope SCOPE_ROLE = new OpenIdScope
        {
            Name = "role",
            Claims = new List<string>
            {
                "role"
            }
        };

        public static List<OpenIdScope> Scopes = new List<OpenIdScope>
        {
            SCOPE_ROLE
        };

        public static List<AuthenticationContextClassReference> AcrLst => new List<AuthenticationContextClassReference>
        {
            new AuthenticationContextClassReference
            {
                DisplayName = "First level of assurance",
                Name = "sid-load-01",
                AuthenticationMethodReferences = new List<string>
                {
                    "pwd"
                }
            }
        };

        public static List<OAuthUser> Users => new List<OAuthUser>
        {
            new OAuthUser
            {
                Id = "administrator",
                Credentials = new List<OAuthUserCredential>
                {
                    new OAuthUserCredential
                    {
                        CredentialType = "pwd",
                        Value = PasswordHelper.ComputeHash("password")
                    }
                },
                Claims = new Dictionary<string, string>
                {
                    { SimpleIdServer.Jwt.Constants.UserClaims.Subject, "administrator" },
                    { SimpleIdServer.Jwt.Constants.UserClaims.GivenName, "administrator" },
                    { SimpleIdServer.Jwt.Constants.UserClaims.Role, "admin" }
                }
            },
            new OAuthUser
            {
                Id = "businessanalyst",
                Credentials = new List<OAuthUserCredential>
                {
                    new OAuthUserCredential
                    {
                        CredentialType = "pwd",
                        Value = PasswordHelper.ComputeHash("password")
                    }
                },
                Claims = new Dictionary<string, string>
                {
                    { SimpleIdServer.Jwt.Constants.UserClaims.Subject, "businessanalyst" },
                    { SimpleIdServer.Jwt.Constants.UserClaims.GivenName, "businessanalyst" },
                    { SimpleIdServer.Jwt.Constants.UserClaims.Role, "businessanalyst" }
                }
            }
        };

        public static List<OpenIdClient> Clients => new List<OpenIdClient>
        {            
            new OpenIdClient
            {
                ClientId = "caseManagementWebsite",
                Secrets = new List<ClientSecret>
                {
                    new ClientSecret(ClientSecretTypes.SharedSecret, PasswordHelper.ComputeHash("b98113b5-f45f-4a4a-9db5-610b7183e148"))
                },
                TokenEndPointAuthMethod = "client_secret_post",
                ApplicationType = "web",
                UpdateDateTime = DateTime.UtcNow,
                CreateDateTime = DateTime.UtcNow,
                TokenExpirationTimeInSeconds = 60 * 30,
                RefreshTokenExpirationTimeInSeconds = 60 * 30,
                TokenSignedResponseAlg = "RS256",
                IdTokenSignedResponseAlg = "RS256",
                AllowedScopes = new List<OpenIdScope>
                {
                    SIDOpenIdConstants.StandardScopes.Profile,
                    SIDOpenIdConstants.StandardScopes.Email,
                    SCOPE_ROLE
                },
                GrantTypes = new List<string>
                {
                    "implicit"
                },
                RedirectionUrls = new List<string>
                {
                    "http://localhost:51724",
                    "http://localhost:8080"
                },
                PreferredTokenProfile = "Bearer",
                ResponseTypes = new List<string>
                {
                    "token",
                    "id_token"
                }
            },
            new OpenIdClient
            {
                ClientId = "caseManagementPerformanceWebsite",
                Secrets = new List<ClientSecret>
                {
                    new ClientSecret(ClientSecretTypes.SharedSecret, PasswordHelper.ComputeHash("91894b86-c57e-489a-838d-fb82621a67ee"))
                },
                TokenEndPointAuthMethod = "client_secret_post",
                ApplicationType = "web",
                UpdateDateTime = DateTime.UtcNow,
                CreateDateTime = DateTime.UtcNow,
                TokenExpirationTimeInSeconds = 60 * 30,
                RefreshTokenExpirationTimeInSeconds = 60 * 30,
                TokenSignedResponseAlg = "RS256",
                IdTokenSignedResponseAlg = "RS256",
                AllowedScopes = new List<OpenIdScope>
                {
                    SIDOpenIdConstants.StandardScopes.Profile,
                    SIDOpenIdConstants.StandardScopes.Email,
                    SCOPE_ROLE
                },
                GrantTypes = new List<string>
                {
                    "implicit"
                },
                RedirectionUrls = new List<string>
                {
                    "http://localhost:51724",
                    "http://localhost:8080"
                },
                PreferredTokenProfile = "Bearer",
                ResponseTypes = new List<string>
                {
                    "token",
                    "id_token"
                }
            }
        };
    }
}