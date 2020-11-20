import { RouterModule } from '@angular/router';
import { CasesComponent } from './cases.component';
import { AuthGuard } from '../infrastructure/auth-guard.service';
var routes = [
    { path: '', redirectTo: 'casefiles', pathMatch: 'full' },
    { path: 'casefiles', component: CasesComponent, loadChildren: './casefiles/casefiles.module#CaseFilesModule', canActivate: [AuthGuard], data: { role: ['businessanalyst'] } },
    { path: 'caseplans', component: CasesComponent, loadChildren: './caseplans/caseplans.module#CasePlansModule', canActivate: [AuthGuard], data: { role: ['businessanalyst'] } },
    { path: 'caseplaninstances', component: CasesComponent, loadChildren: './caseplaninstances/caseplaninstances.module#CasePlanInstancesModule', canActivate: [AuthGuard], data: { role: ['businessanalyst', 'caseworker'] } }
];
export var CasesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=cases.routes.js.map