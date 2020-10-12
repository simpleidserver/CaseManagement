import { RouterModule, Routes } from '@angular/router';
import { ViewHumanTaskDefDeadlinesComponent } from './deadlines/deadlines.component';
import { ViewHumanTaskDefInfoComponent } from './info/info.component';
import { ViewPresentationParametersComponent } from './presentationparameters/view-presentationparameters.component';
import { ViewHumanTaskDefRenderingComponent } from './rendering/rendering.component';
import { ViewTaskPeopleAssignmentComponent } from './peopleassignment/view-peopleassignment.component';


const routes: Routes = [
    { path: '', redirectTo: 'task', pathMatch: 'full' },
    { path: 'task', component: ViewHumanTaskDefInfoComponent },
    { path: 'deadlines', component: ViewHumanTaskDefDeadlinesComponent },
    { path: 'presentationelements', component: ViewPresentationParametersComponent },
    { path: 'rendering', component: ViewHumanTaskDefRenderingComponent },
    { path: 'peopleassignment', component: ViewTaskPeopleAssignmentComponent }
];

export const HumanTaskDefsViewRoutes = RouterModule.forChild(routes);