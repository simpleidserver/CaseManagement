import { RouterModule, Routes } from '@angular/router';
import { ListRolesComponent } from './list/list.component';
import { ViewRoleComponent } from './view/view.component';

const routes: Routes = [
    { path: '', component: ListRolesComponent },
    { path: ':id', component: ViewRoleComponent }
];

export const RolesRoutes = RouterModule.forChild(routes);