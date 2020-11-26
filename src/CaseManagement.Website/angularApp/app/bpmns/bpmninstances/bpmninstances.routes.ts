import { RouterModule, Routes } from '@angular/router';
import { ListBpmnInstancesComponent } from './list/list.component';
import { ViewBpmnInstanceComponent } from './view/view.component';

const routes: Routes = [
    { path: '', component: ListBpmnInstancesComponent },
    { path: ':id', component: ViewBpmnInstanceComponent }
];

export const BpmnInstancesRoutes = RouterModule.forChild(routes);