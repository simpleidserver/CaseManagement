import { RouterModule } from '@angular/router';
import { ViewHumanTaskDef } from './view/view.component';
var routes = [
    { path: ':id', component: ViewHumanTaskDef }
];
export var HumanTaskDefsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=humantaskdefs.routes.js.map