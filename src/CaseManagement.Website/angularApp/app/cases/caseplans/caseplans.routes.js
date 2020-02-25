import { RouterModule } from '@angular/router';
import { ListCasePlansComponent } from './list/list.component';
import { ViewCaseDefinitionComponent } from './view/view.component';
import { HistoryCasePlanComponent } from './history/history.component';
var routes = [
    { path: '', component: ListCasePlansComponent },
    { path: ':id', component: ViewCaseDefinitionComponent },
    { path: ':id/history', component: HistoryCasePlanComponent }
];
export var CasePlansRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=caseplans.routes.js.map