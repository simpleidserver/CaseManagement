import { RouterModule } from '@angular/router';
import { ListCaseFilesComponent } from './list/list.component';
import { ViewCaseFilesComponent } from './view/view.component';
var routes = [
    { path: '', component: ListCaseFilesComponent },
    { path: ':id', component: ViewCaseFilesComponent }
];
export var CaseFilesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=casefiles.routes.js.map