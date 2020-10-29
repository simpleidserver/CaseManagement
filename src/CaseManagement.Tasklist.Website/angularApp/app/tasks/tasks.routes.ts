import { RouterModule, Routes } from '@angular/router';
import { ListTasksComponent } from './list/list.component';
import { ViewTaskComponent } from './view/view.component';


const routes: Routes = [
    { path: '', component: ListTasksComponent },
    { path: ':id', component: ViewTaskComponent }
];

export const HomeRoutes = RouterModule.forChild(routes);