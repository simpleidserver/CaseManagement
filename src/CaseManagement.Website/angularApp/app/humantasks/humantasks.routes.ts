import { RouterModule, Routes } from '@angular/router';
import { HumanTasksComponent } from './humantasks.component';
import { ViewHumanTaskDefInfoComponent } from './viewdef/info/info.component';
import { ViewHumanTaskDefRenderingComponent } from './viewdef/rendering/rendering.component';
import { ViewHumanTaskDef } from './viewdef/view.component';
import { ViewDeadlineComponent } from './viewdeadline/viewdeadline.component';
import { ViewEscalationComponent } from './viewdeadline/viewescalation/viewescalation.component';

const routes: Routes = [
    { path: '', component: HumanTasksComponent },
    {
        path: ':id/deadline/:deadlineid', component: ViewDeadlineComponent, children: [
            { path: ':escalationid', component: ViewEscalationComponent }
        ],
    },
    {
        path: ':id', component: ViewHumanTaskDef, children: [
            { path: '', redirectTo: 'task', pathMatch: 'full' },
            { path: 'task', component: ViewHumanTaskDefInfoComponent },
            { path: 'rendering', component: ViewHumanTaskDefRenderingComponent }
        ]
    }
];

export const HumanTasksRoutes = RouterModule.forChild(routes);