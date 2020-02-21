import { RouterModule, Routes } from '@angular/router';
import { ListCasePlanInstancesComponent } from './list/list.component';

const routes: Routes = [
    { path: '', component: ListCasePlanInstancesComponent }
];

export const CasePlanInstancesRoutes = RouterModule.forChild(routes);