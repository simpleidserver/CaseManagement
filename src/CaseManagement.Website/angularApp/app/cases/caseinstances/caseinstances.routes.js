import { RouterModule } from '@angular/router';
import { ListCaseInstancesComponent } from './list/list.component';
import { ViewCaseInstanceComponent } from './view/view.component';
var routes = [
    { path: '', component: ListCaseInstancesComponent },
    { path: ':id', component: ViewCaseInstanceComponent }
];
export var CaseInstancesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=caseinstances.routes.js.map