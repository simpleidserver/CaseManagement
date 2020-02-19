import { RouterModule } from '@angular/router';
import { ListCaseFilesComponent } from './list/list.component';
import { ViewCaseFilesComponent } from './view/view.component';
import { HistoryCaseFileComponent } from './history/history.component';
var routes = [
    { path: '', component: ListCaseFilesComponent },
    { path: ':id', component: ViewCaseFilesComponent },
    { path: ':id/history', component: HistoryCaseFileComponent }
];
export var CaseFilesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=casefiles.routes.js.map