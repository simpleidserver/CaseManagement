import { RouterModule } from '@angular/router';
import { ListCaseDefinitionsComponent } from './list/list.component';
import { ViewCaseDefinitionComponent } from './view/view.component';
var routes = [
    { path: '', component: ListCaseDefinitionsComponent },
    { path: ':id', component: ViewCaseDefinitionComponent }
];
export var CaseDefinitionsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=casedefinitions.routes.js.map