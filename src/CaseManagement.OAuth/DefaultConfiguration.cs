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
                Name = "get_statistic",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "get_performance",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "get_caseplan",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "add_casefile",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "update_casefile",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "publish_casefile",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "add_case_instance",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "launch_case_intance",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "get_casefile",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "search_caseplaninstance",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "add_case_instance",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "launch_caseplaninstance",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "get_forminstances",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "get_caseworkertasks",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "get_caseplaninstance",
                IsExposedInConfigurationEdp = true
            },
            new OAuthScope
            {
                Name = "activate_caseplaninstance",
                IsExposedInConfigurationEdp = true
            }
        };

        public static List<OAuthClient> Clients => new List<OAuthClient>
        {
            new OAuthClient
            {
                ClientId = "websiteGateway",
                Secrets = new List<ClientSecret>
                {
                    new ClientSecret(ClientSecretTypes.SharedSecret, PasswordHelper.ComputeHash("websiteGatewaySecret"))
                },
                ClientNames = new []
                {
                    new OAuthTranslation("websiteGateway_client_name", "Website gateway", "fr")
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
                        Name = "get_statistic"
                    },
                    new OAuthScope
                    {
                        Name = "get_performance"
                    },
                    new OAuthScope
                    {
                        Name = "get_caseplan"
                    },
                    new OAuthScope
                    {
                        Name = "add_casefile"
                    },
                    new OAuthScope
                    {
                        Name = "update_casefile"
                    },
                    new OAuthScope
                    {
                        Name = "publish_casefile"
                    },
                    new OAuthScope
                    {
                        Name = "add_case_instance"
                    },
                    new OAuthScope
                    {
                        Name = "launch_case_intance"
                    },
                    new OAuthScope
                    {
                        Name = "get_casefile"
                    },
                    new OAuthScope
                    {
                        Name = "search_caseplaninstance"
                    },
                    new OAuthScope
                    {
                        Name = "add_case_instance"
                    },
                    new OAuthScope
                    {
                        Name = "launch_caseplaninstance"
                    },
                    new OAuthScope
                    {
                        Name = "get_forminstances"
                    },
                    new OAuthScope
                    {
                        Name = "get_caseworkertasks"
                    },
                    new OAuthScope
                    {
                        Name = "get_caseplaninstance"
                    },
                    new OAuthScope
                    {
                        Name = "activate_caseplaninstance"
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