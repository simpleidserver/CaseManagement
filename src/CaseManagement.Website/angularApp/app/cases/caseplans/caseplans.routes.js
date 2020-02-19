import { RouterModule } from '@angular/router';
import { ListCasePlansComponent } from './list/list.component';
import { ViewCaseDefinitionComponent } from './view/view.component';
var routes = [
    { path: '', component: ListCasePlansComponent },
    { path: ':id', component: ViewCaseDefinitionComponent }
];
export var CasePlansRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=caseplans.routes.js.map