import { RouterModule, Routes } from '@angular/router';

import { StatisticsComponent } from './components/statistics.component';

const routes: Routes = [
    { path: '', component: StatisticsComponent }
];

export const StatisticsRoutes = RouterModule.forChild(routes);