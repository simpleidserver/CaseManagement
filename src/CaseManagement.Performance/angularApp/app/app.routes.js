import { AuthGuard } from './infrastructure/auth-guard.service';
export var routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', loadChildren: './home/home.module#HomeModule' },
    { path: 'statistics', loadChildren: './statistics/statistics.module#StatisticsModule', canActivate: [AuthGuard], data: { role: 'admin' } },
    { path: 'performances', loadChildren: './performances/performances.module#PerformancesModule', canActivate: [AuthGuard], data: { role: 'admin' } },
    { path: 'status', loadChildren: './status/status.module#StatusModule' },
    { path: '**', redirectTo: '/status/404' }
];
//# sourceMappingURL=app.routes.js.map