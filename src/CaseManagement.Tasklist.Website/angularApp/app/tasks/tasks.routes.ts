import { RouterModule, Routes } from '@angular/router';
import { ListTasksComponent } from './list/list.component';


const routes: Routes = [
    { path: '', component: ListTasksComponent }
];

export const HomeRoutes = RouterModule.forChild(routes);