import { RouterModule } from '@angular/router';
import { CasesComponent } from './cases.component';
var routes = [
    { path: '', redirectTo: 'casefiles', pathMatch: 'full' },
    { path: 'casefiles', component: CasesComponent, loadChildren: './casefiles/casefiles.module#CaseFilesModule' },
    { path: 'caseplans', component: CasesComponent, loadChildren: './caseplans/caseplans.module#CasePlansModule' },
    { path: 'caseplaninstances', component: CasesComponent, loadChildren: './caseplaninstances/caseplaninstances.module#CasePlanInstancesModule' }
];
export var CasesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=cases.routes.js.map