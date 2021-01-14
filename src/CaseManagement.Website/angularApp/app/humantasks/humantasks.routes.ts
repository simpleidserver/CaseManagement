import { RouterModule, Routes } from '@angular/router';
import { HumanTasksComponent } from './humantasks.component';
import { ViewHumanTaskDefInfoComponent } from './viewdef/info/info.component';
import { ViewHumanTaskDefRenderingComponent } from './viewdef/rendering/rendering.component';
import { ViewHumanTaskDef } from './viewdef/view.component';

const routes: Routes = [
    { path: '', component: HumanTasksComponent },
    {
        path: ':id', component: ViewHumanTaskDef, children: [
            { path: '', redirectTo: 'task', pathMatch: 'full' },
            { path: 'task', component: ViewHumanTaskDefInfoComponent },
            { path: 'rendering', component: ViewHumanTaskDefRenderingComponent }
        ]
    }
];

export const HumanTasksRoutes = RouterModule.forChild(routes);