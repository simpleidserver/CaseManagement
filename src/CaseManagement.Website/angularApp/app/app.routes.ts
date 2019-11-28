import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'casedefinitions', loadChildren: './casedefinitions/casedefinitions.module#CaseDefinitionsModule' },
    { path: 'caseinstances', loadChildren: './caseinstances/caseinstances.module#CaseInstancesModule' },
    { path: '**', redirectTo: '/status/404' }
];