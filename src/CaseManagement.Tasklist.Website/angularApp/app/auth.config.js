export var authConfig = {
    issuer: process.env.OPENID_URL,
    clientId: 'caseManagementTasklistWebsite',
    scope: 'openid profile email role',
    redirectUri: process.env.REDIRECT_URL,
    responseType: 'code',
    requireHttps: false
};
//# sourceMappingURL=auth.config.js.map