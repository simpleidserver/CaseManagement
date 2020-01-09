import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'casefiles', loadChildren: './casefiles/casefiles.module#CaseFilesModule' },
    { path: '**', redirectTo: '/status/404' }
];