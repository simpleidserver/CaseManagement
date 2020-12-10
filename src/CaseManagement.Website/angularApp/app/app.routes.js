import { AuthGuard } from './infrastructure/auth-guard.service';
export var routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', loadChildren: './home/home.module#HomeModule' },
    { path: 'cmmns', loadChildren: './cmmns/cmmns.module#CmmnsModule', canActivate: [AuthGuard], data: { role: ['businessanalyst', 'caseworker'] } },
    { path: 'bpmns', loadChildren: './bpmns/bpmns.module#BpmnsModule', canActivate: [AuthGuard], data: { role: ['businessanalyst', 'caseworker'] } },
    { path: 'humantasks', loadChildren: './humantasks/humantasks.module#HumanTasksModule', canActivate: [AuthGuard], data: { role: ['businessanalyst', 'caseworker'] } },
    { path: 'status', loadChildren: './status/status.module#StatusModule' },
    { path: '**', redirectTo: '/status/404' }
];
//# sourceMappingURL=app.routes.js.map