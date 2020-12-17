import { RouterModule, Routes } from '@angular/router';
import { HumanTasksComponent } from './humantasks.component';
import { ViewHumanTaskDef } from './viewdef/view.component';
import { ViewHumanTaskDefInfoComponent } from './viewdef/info/info.component';
import { ViewTaskPeopleAssignmentComponent } from './viewdef/peopleassignment/view-peopleassignment.component';
import { ViewPresentationParametersComponent } from './viewdef/presentationparameters/view-presentationparameters.component';
import { ViewHumanTaskDefDeadlinesComponent } from './viewdef/deadlines/deadlines.component';
import { ViewHumanTaskDefRenderingComponent } from './viewdef/rendering/rendering.component';

const routes: Routes = [
    { path: '', component: HumanTasksComponent },
    {
        path: ':id', component: ViewHumanTaskDef, children: [
            { path: '', redirectTo: 'task', pathMatch: 'full' },
            { path: 'task', component: ViewHumanTaskDefInfoComponent },
            { path: 'peopleassignment', component: ViewTaskPeopleAssignmentComponent },
            { path: 'presentationelements', component: ViewPresentationParametersComponent },
            { path: 'deadlines', component: ViewHumanTaskDefDeadlinesComponent },
            { path: 'rendering', component: ViewHumanTaskDefRenderingComponent }
        ]
    }
];

export const HumanTasksRoutes = RouterModule.forChild(routes);