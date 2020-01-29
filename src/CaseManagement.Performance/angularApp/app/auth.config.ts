  
import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
    issuer: 'http://localhost:60000',
    clientId: 'caseManagementPerformanceWebsite',
    scope: 'openid profile email role',
    redirectUri: window.location.origin,
    disableAtHashCheck: true
}