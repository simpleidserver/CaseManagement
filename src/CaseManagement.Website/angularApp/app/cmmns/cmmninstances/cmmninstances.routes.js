import { RouterModule } from '@angular/router';
import { ViewCmmnPlanComponent } from './view/view.component';
import { ViewCmmnPlanInformationComponent } from './view/information/information.component';
import { ViewCmmnPlanInstancesComponent } from './view/instances/instances.component';
var routes = [
    {
        path: ':id', component: ViewCmmnPlanComponent, children: [
            { path: '', redirectTo: 'info', pathMatch: 'full' },
            { path: 'info', component: ViewCmmnPlanInformationComponent },
            { path: 'instances', component: ViewCmmnPlanInstancesComponent }
        ]
    }
];
export var CmmnPlansRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=cmmnplans.routes.js.map