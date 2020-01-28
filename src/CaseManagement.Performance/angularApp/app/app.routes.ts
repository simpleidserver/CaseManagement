import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'performances', loadChildren: './performances/performances.module#PerformancesModule' },
    { path: '**', redirectTo: '/status/404' }
];