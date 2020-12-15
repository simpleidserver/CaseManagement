import { RouterModule } from '@angular/router';
import { BpmnsComponent } from './bpmns.component';
import { ViewBpmnFileComponent } from './viewfile/viewfile.component';
import { ViewBpmnInstanceComponent } from './viewinstance/view.component';
var routes = [
    { path: '', component: BpmnsComponent },
    { path: ':id', component: ViewBpmnFileComponent },
    { path: ':id/:instanceid', component: ViewBpmnInstanceComponent },
    { path: ':id/:instanceid/:pathid', component: ViewBpmnInstanceComponent }
];
export var BpmnsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=bpmns.routes.js.map