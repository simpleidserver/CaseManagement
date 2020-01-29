import { Routes } from '@angular/router';
import { AuthGuard } from './infrastructure/auth-guard.service';

export const routes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', loadChildren: './home/home.module#HomeModule' },
    { path: 'statistics', loadChildren: './statistics/statistics.module#StatisticsModule', canActivate: [AuthGuard], data: { role: 'admin' } },
    { path: 'performances', loadChildren: './performances/performances.module#PerformancesModule', canActivate: [AuthGuard], data: { role: 'admin' } },
    { path: 'status', loadChildren: './status/status.module#StatusModule' },
    { path: '**', redirectTo: '/status/404' }
];