import { RouterModule, Routes } from '@angular/router';
import { CasesComponent } from './cases.component';

const routes: Routes = [
    { path: '', redirectTo: 'casefiles', pathMatch: 'full' },
    { path: 'casefiles', component: CasesComponent, loadChildren: './casefiles/casefiles.module#CaseFilesModule' },
    { path: 'casedefinitions', component: CasesComponent, loadChildren: './casedefinitions/casedefinitions.module#CaseDefinitionsModule' },
    { path: 'caseinstances', component: CasesComponent, loadChildren: './caseinstances/caseinstances.module#CaseInstancesModule' }
];

export const CasesRoutes = RouterModule.forChild(routes);