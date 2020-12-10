import { RouterModule } from '@angular/router';
import { ListBpmnInstancesComponent } from './list/list.component';
var routes = [
    { path: '', component: ListBpmnInstancesComponent },
    { path: ':id', loadChildren: './view/view.module#ViewBpmnInstanceModule' }
];
export var BpmnInstancesRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=bpmninstances.routes.js.map