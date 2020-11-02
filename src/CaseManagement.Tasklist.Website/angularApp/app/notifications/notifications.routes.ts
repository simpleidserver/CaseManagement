import { RouterModule, Routes } from '@angular/router';
import { ListNotificationsComponent } from './list/list.component';


const routes: Routes = [
    { path: '', component: ListNotificationsComponent }
];

export const NotificationsRoutes = RouterModule.forChild(routes);