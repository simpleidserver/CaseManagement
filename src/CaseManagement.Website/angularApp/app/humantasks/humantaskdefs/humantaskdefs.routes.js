import { RouterModule } from '@angular/router';
import { ViewHumanTaskDef } from './view/view.component';
var routes = [
    { path: ':id', component: ViewHumanTaskDef, loadChildren: './view/view.module#HumanTaskDefsViewModule' }
];
export var HumanTaskDefsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=humantaskdefs.routes.js.map