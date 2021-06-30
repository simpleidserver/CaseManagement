import { RouterModule, Routes } from '@angular/router';
import { BpmnsComponent } from './bpmns.component';
import { ViewDelegateConfigurationComponent } from './viewdelegate/viewdelegate.component';
import { ViewBpmnFileComponent } from './viewfile/viewfile.component';
import { ViewBpmnInstanceComponent } from './viewinstance/view.component';

const routes: Routes = [
    { path: '', component: BpmnsComponent },
    { path: 'delegateconfigurations/:id', component: ViewDelegateConfigurationComponent },
    { path: ':id', component: ViewBpmnFileComponent },
    { path: ':id/:instanceid', component: ViewBpmnInstanceComponent },
    { path: ':id/:instanceid/:pathid', component: ViewBpmnInstanceComponent }
];

export const BpmnsRoutes = RouterModule.forChild(routes);