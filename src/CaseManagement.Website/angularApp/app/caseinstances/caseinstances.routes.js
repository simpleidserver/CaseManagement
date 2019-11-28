import { RouterModule } from '@angular/router';
import { CaseInstanceComponent } from './case-instance/case-instance.component';
var routes = [
    { path: ':id', component: CaseInstanceComponent }
];
export var CaseInstancesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=caseinstances.routes.js.map