// Copyright (c) SimpleIdServer. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using CaseManagement.HumanTask;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizationOptionsExtensions
    {
        public static void AddDefaultHumanTaskAuthorizationPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy("Authenticated", p => p.RequireAuthenticatedUser());
            opts.AddPolicy(HumanTaskConstants.ScopeNames.ManageHumanTaskInstance, p =>
            {
                p.RequireClaim("scope", HumanTaskConstants.ScopeNames.ManageHumanTaskInstance);
            });
        }
    }
}