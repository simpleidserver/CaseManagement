import { RouterModule } from '@angular/router';
import { ListCasePlanInstancesComponent } from './list/list.component';
import { ViewCasePlanInstanceComponent } from './view/view.component';
var routes = [
    { path: '', component: ListCasePlanInstancesComponent },
    { path: ':id', component: ViewCasePlanInstanceComponent }
];
export var CasePlanInstancesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=caseplaninstances.routes.js.map