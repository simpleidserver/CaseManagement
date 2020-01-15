import { RouterModule, Routes } from '@angular/router';

import { ListPerformanceComponent } from './list/list.component';

const routes: Routes = [
    { path: '', component: ListPerformanceComponent }
];

export const PerformancesRoutes = RouterModule.forChild(routes);