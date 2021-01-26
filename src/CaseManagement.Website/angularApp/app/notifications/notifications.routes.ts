import { RouterModule, Routes } from '@angular/router';
import { NotificationsComponent } from './notifications.component';
import { ViewNotificationDef } from './viewdef/view.component';
import { ViewNotificationDefInfoComponent } from './viewdef/info/info.component';

const routes: Routes = [
    { path: '', component: NotificationsComponent },
    {
        path: ':id', component: ViewNotificationDef, children: [
            { path: '', redirectTo: 'task', pathMatch: 'full' },
            { path: 'task', component: ViewNotificationDefInfoComponent }
        ]
    }
];

export const NotificationsRoutes = RouterModule.forChild(routes);