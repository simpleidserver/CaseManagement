import { RouterModule, Routes } from '@angular/router';
import { ListCasePlanInstancesComponent } from './list/list.component';
import { ViewCasePlanInstanceComponent } from './view/view.component';

const routes: Routes = [
    { path: '', component: ListCasePlanInstancesComponent },
    { path: ':id', component: ViewCasePlanInstanceComponent }
];

export const CasePlanInstancesRoutes = RouterModule.forChild(routes);