import { RouterModule } from '@angular/router';
import { ViewHumanTaskDef } from './view/view.component';
import { ListHumanTaskDef } from './list/list.component';
var routes = [
    { path: ':id', component: ViewHumanTaskDef, loadChildren: './view/view.module#HumanTaskDefsViewModule' },
    { path: '', component: ListHumanTaskDef }
];
export var HumanTaskDefsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=humantaskdefs.routes.js.map