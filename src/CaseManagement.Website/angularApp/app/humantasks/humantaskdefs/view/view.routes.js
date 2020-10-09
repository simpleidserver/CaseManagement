import { RouterModule } from '@angular/router';
import { ViewHumanTaskDefDeadlinesComponent } from './deadlines/deadlines.component';
import { ViewHumanTaskDefInfoComponent } from './info/info.component';
import { ViewHumanTaskDefRenderingComponent } from './rendering/rendering.component';
var routes = [
    { path: '', redirectTo: 'task', pathMatch: 'full' },
    { path: 'task', component: ViewHumanTaskDefInfoComponent },
    { path: 'deadlines', component: ViewHumanTaskDefDeadlinesComponent },
    { path: 'rendering', component: ViewHumanTaskDefRenderingComponent }
];
export var HumanTaskDefsViewRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=view.routes.js.map