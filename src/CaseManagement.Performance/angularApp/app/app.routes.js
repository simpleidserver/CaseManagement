export var routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'performances', loadChildren: './performances/performances.module#PerformancesModule' },
    { path: '**', redirectTo: '/status/404' }
];
//# sourceMappingURL=app.routes.js.map