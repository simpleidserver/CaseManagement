// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using SimpleIdServer.OAuth.Domains;
using SimpleIdServer.OAuth.Helpers;
using System;
using System.Collections.Generic;

namespace CaseManagement.OAuth
{
    public class DefaultConfiguration
    {
        public static List<OAuthScope> Scopes => new List<OAuthScope>
        {
            new OAuthScope
            {
                Name = "manage_humantaskinstance",
                IsExposedInConfigurationEdp = true
            }
        };

        public static List<OAuthClient> Clients => new List<OAuthClient>
        {
            new OAuthClient
            {
                ClientId = "bpmnClient",
                Secrets = new List<ClientSecret>
                {
                    new ClientSecret(ClientSecretTypes.SharedSecret, PasswordHelper.ComputeHash("bpmnClientSecret"), null)
                },
                ClientNames = new []
                {
                    new OAuthTranslation("bpmnClient_client_name", "BPMN Client", "fr")
                },
                TokenEndPointAuthMethod = "client_secret_post",
                UpdateDateTime = DateTime.UtcNow,
                CreateDateTime = DateTime.UtcNow,
                TokenExpirationTimeInSeconds = 60 * 30,
                RefreshTokenExpirationTimeInSeconds = 60 * 30,
                TokenSignedResponseAlg = "RS256",
                AllowedScopes = new List<OAuthScope>
                {
                    new OAuthScope
                    {
                        Name = "create_humantaskinstance"
                    }
                },
                GrantTypes = new List<string>
                {
                    "client_credentials"
                },
                PreferredTokenProfile = "Bearer"
            },
            new OAuthClient
            {
                ClientId = "cmmnClient",
                Secrets = new List<ClientSecret>
                {
                    new ClientSecret(ClientSecretTypes.SharedSecret, PasswordHelper.ComputeHash("cmmnClientSecret"), null)
                },
                ClientNames = new []
                {
                    new OAuthTranslation("cmmnClient_client_name", "CMMN Client", "fr")
                },
                TokenEndPointAuthMethod = "client_secret_post",
                UpdateDateTime = DateTime.UtcNow,
                CreateDateTime = DateTime.UtcNow,
                TokenExpirationTimeInSeconds = 60 * 30,
                RefreshTokenExpirationTimeInSeconds = 60 * 30,
                TokenSignedResponseAlg = "RS256",
                AllowedScopes = new List<OAuthScope>
                {
                    new OAuthScope
                    {
                        Name = "create_humantaskinstance"
                    }
                },
                GrantTypes = new List<string>
                {
                    "client_credentials"
                },
                PreferredTokenProfile = "Bearer"
            },
            new OAuthClient
            {
                ClientId = "humanTaskClient",
                Secrets = new List<ClientSecret>
                {
                    new ClientSecret(ClientSecretTypes.SharedSecret, PasswordHelper.ComputeHash("humanTaskClientSecret"), null)
                },
                ClientNames = new []
                {
                    new OAuthTranslation("humanTaskClient_client_name", "HumanTask Client", "fr")
                },
                TokenEndPointAuthMethod = "client_secret_post",
                UpdateDateTime = DateTime.UtcNow,
                CreateDateTime = DateTime.UtcNow,
                TokenExpirationTimeInSeconds = 60 * 30,
                RefreshTokenExpirationTimeInSeconds = 60 * 30,
                TokenSignedResponseAlg = "RS256",
                AllowedScopes = new List<OAuthScope>
                {
                    new OAuthScope
                    {
                        Name = "complete_humantask"
                    }
                },
                GrantTypes = new List<string>
                {
                    "client_credentials"
                },
                PreferredTokenProfile = "Bearer"
            }
        };
    }
}