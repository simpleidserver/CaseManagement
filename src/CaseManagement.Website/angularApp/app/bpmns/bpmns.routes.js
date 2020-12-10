import { RouterModule } from '@angular/router';
import { BpmnsComponent } from './bpmns.component';
var routes = [
    { path: '', redirectTo: 'bpmnfiles', pathMatch: 'full' },
    { path: 'bpmnfiles', component: BpmnsComponent, loadChildren: './bpmnfiles/bpmnfiles.module#BpmnFilesModule' },
    { path: 'bpmninstances', component: BpmnsComponent, loadChildren: './bpmninstances/bpmninstances.module#BpmnInstancesModule' }
];
export var BpmnsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=bpmns.routes.js.map