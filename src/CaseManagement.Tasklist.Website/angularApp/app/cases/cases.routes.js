import { RouterModule } from '@angular/router';
import { ListCasesComponent } from './list/list.component';
import { ViewCaseComponent } from './view/view.component';
import { ViewFormComponent } from './view/viewform.component';
var routes = [
    { path: '', component: ListCasesComponent },
    {
        path: ':id', component: ViewCaseComponent, children: [
            { path: ':formid', component: ViewFormComponent }
        ]
    }
];
export var CaseRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=cases.routes.js.map