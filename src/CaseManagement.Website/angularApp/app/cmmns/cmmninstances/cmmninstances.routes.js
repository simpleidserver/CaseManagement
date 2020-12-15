import { RouterModule } from '@angular/router';
import { ViewCmmnPlanInstanceComponent } from './view/view.component';
import { ViewCasePlanEltInstanceComponent } from './view/viewelt.component';
import { ViewTransitionHistoriesComponent } from './view/viewtransitionhistories.component';
var routes = [
    {
        path: ':id', component: ViewCmmnPlanInstanceComponent, children: [
            {
                path: ':eltid', component: ViewCasePlanEltInstanceComponent, children: [
                    { path: ':instid/history', component: ViewTransitionHistoriesComponent },
                ]
            }
        ]
    }
];
export var CmmnPlansRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=cmmninstances.routes.js.map