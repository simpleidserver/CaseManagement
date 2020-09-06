import { RouterModule } from '@angular/router';
import { ListRolesComponent } from './list/list.component';
import { ViewRoleComponent } from './view/view.component';
var routes = [
    { path: '', component: ListRolesComponent },
    { path: ':id', component: ViewRoleComponent }
];
export var RolesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=roles.routes.js.map