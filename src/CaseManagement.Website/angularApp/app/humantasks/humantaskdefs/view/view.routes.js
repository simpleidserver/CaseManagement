import { RouterModule } from '@angular/router';
import { ViewHumanTaskDefDeadlinesComponent } from './deadlines/deadlines.component';
import { ViewHumanTaskDefInfoComponent } from './info/info.component';
import { ViewPresentationParametersComponent } from './presentationparameters/view-presentationparameters.component';
import { ViewHumanTaskDefRenderingComponent } from './rendering/rendering.component';
import { ViewTaskPeopleAssignmentComponent } from './peopleassignment/view-peopleassignment.component';
var routes = [
    { path: '', redirectTo: 'task', pathMatch: 'full' },
    { path: 'task', component: ViewHumanTaskDefInfoComponent },
    { path: 'deadlines', component: ViewHumanTaskDefDeadlinesComponent },
    { path: 'presentationelements', component: ViewPresentationParametersComponent },
    { path: 'rendering', component: ViewHumanTaskDefRenderingComponent },
    { path: 'peopleassignment', component: ViewTaskPeopleAssignmentComponent }
];
export var HumanTaskDefsViewRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=view.routes.js.map