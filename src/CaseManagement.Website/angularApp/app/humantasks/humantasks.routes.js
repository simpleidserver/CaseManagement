import { RouterModule } from '@angular/router';
import { HumanTasksComponent } from './humantasks.component';
import { ViewHumanTaskDef } from './viewdef/view.component';
import { ViewHumanTaskDefInfoComponent } from './viewdef/info/info.component';
import { ViewTaskPeopleAssignmentComponent } from './viewdef/peopleassignment/view-peopleassignment.component';
import { ViewPresentationParametersComponent } from './viewdef/presentationparameters/view-presentationparameters.component';
import { ViewHumanTaskDefDeadlinesComponent } from './viewdef/deadlines/deadlines.component';
import { ViewHumanTaskDefRenderingComponent } from './viewdef/rendering/rendering.component';
var routes = [
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
export var HumanTasksRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=humantasks.routes.js.map