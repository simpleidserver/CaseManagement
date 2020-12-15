import { RouterModule, Routes } from '@angular/router';
import { BpmnsComponent } from './bpmns.component';
import { ViewBpmnFileComponent } from './viewfile/viewfile.component';
import { ViewBpmnInstanceComponent } from './viewinstance/view.component';

const routes: Routes = [
    { path: '', component: BpmnsComponent },
    { path: ':id', component: ViewBpmnFileComponent },
    { path: ':id/:instanceid', component: ViewBpmnInstanceComponent },
    { path: ':id/:instanceid/:pathid', component: ViewBpmnInstanceComponent }
];

export const BpmnsRoutes = RouterModule.forChild(routes);