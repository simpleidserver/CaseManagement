import { RouterModule, Routes } from '@angular/router';
import { CasesComponent } from './cases.component';

const routes: Routes = [
    { path: '', redirectTo: 'casefiles', pathMatch: 'full' },
    { path: 'casefiles', component: CasesComponent, loadChildren: './casefiles/casefiles.module#CaseFilesModule' },
    { path: 'caseplans', component: CasesComponent, loadChildren: './caseplans/caseplans.module#CasePlansModule' },
    { path: 'caseplaninstances', component: CasesComponent, loadChildren: './caseplaninstances/caseplaninstances.module#CasePlanInstancesModule' }
];

export const CasesRoutes = RouterModule.forChild(routes);