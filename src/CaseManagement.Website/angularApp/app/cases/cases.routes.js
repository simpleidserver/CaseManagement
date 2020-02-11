import { RouterModule } from '@angular/router';
import { CasesComponent } from './cases.component';
var routes = [
    { path: '', redirectTo: 'casefiles', pathMatch: 'full' },
    { path: 'casefiles', component: CasesComponent, loadChildren: './casefiles/casefiles.module#CaseFilesModule' },
    { path: 'casedefinitions', component: CasesComponent, loadChildren: './casedefinitions/casedefinitions.module#CaseDefinitionsModule' },
    { path: 'caseinstances', component: CasesComponent, loadChildren: './caseinstances/caseinstances.module#CaseInstancesModule' }
];
export var CasesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=cases.routes.js.map