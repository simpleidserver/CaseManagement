import { RouterModule, Routes } from '@angular/router';
import { CasesComponent } from './cases.component';

const routes: Routes = [
    { path: '', redirectTo: 'casefiles', pathMatch: 'full' },
    { path: 'casefiles', component: CasesComponent, loadChildren: './casefiles/casefiles.module#CaseFilesModule' },
];

export const CasesRoutes = RouterModule.forChild(routes);