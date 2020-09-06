export var authConfig = {
    issuer: process.env.OPENID_URL,
    clientId: 'caseManagementWebsite',
    scope: 'openid profile email role',
    redirectUri: process.env.REDIRECT_URL,
    requireHttps: false
};
//# sourceMappingURL=auth.config.js.map