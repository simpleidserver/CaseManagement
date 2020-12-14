import { RouterModule, Routes } from '@angular/router';
import { BpmnsComponent } from './bpmns.component';
import { ViewBpmnFileComponent } from './viewfile/viewfile.component';

const routes: Routes = [
    { path: '', component: BpmnsComponent },
    { path: ':id', component: ViewBpmnFileComponent }
];

export const BpmnsRoutes = RouterModule.forChild(routes);