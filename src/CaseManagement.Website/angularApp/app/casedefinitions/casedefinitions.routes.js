import { RouterModule } from '@angular/router';
import { ListCaseDefsComponent } from './list-case-defs/list-case-defs.component';
import { CaseDefComponent } from './case-def/case-def.component';
var routes = [
    { path: '', component: ListCaseDefsComponent },
    { path: ':id', component: CaseDefComponent }
];
export var CaseDefinitionsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=casedefinitions.routes.js.map