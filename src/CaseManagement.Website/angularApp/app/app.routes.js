export var routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'casedefinitions', loadChildren: './casedefinitions/casedefinitions.module#CaseDefinitionsModule' },
    { path: '**', redirectTo: '/status/404' }
];
//# sourceMappingURL=app.routes.js.map