import { RouterModule, Routes } from '@angular/router';
import { ViewHumanTaskDefDeadlinesComponent } from './deadlines/deadlines.component';
import { ViewHumanTaskDefInfoComponent } from './info/info.component';
import { ViewHumanTaskDefRenderingComponent } from './rendering/rendering.component';


const routes: Routes = [
    { path: '', redirectTo: 'task', pathMatch: 'full' },
    { path: 'task', component: ViewHumanTaskDefInfoComponent },
    { path: 'deadlines', component: ViewHumanTaskDefDeadlinesComponent },
    { path: 'rendering', component: ViewHumanTaskDefRenderingComponent }
];

export const HumanTaskDefsViewRoutes = RouterModule.forChild(routes);