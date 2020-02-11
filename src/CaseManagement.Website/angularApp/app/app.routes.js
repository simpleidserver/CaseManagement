import { AuthGuard } from './infrastructure/auth-guard.service';
export var routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', loadChildren: './home/home.module#HomeModule' },
    { path: 'cases', loadChildren: './cases/cases.module#CasesModule', canActivate: [AuthGuard], data: { role: 'businessanalyst' } },
    { path: 'status', loadChildren: './status/status.module#StatusModule' },
    { path: '**', redirectTo: '/status/404' }
];
//# sourceMappingURL=app.routes.js.map