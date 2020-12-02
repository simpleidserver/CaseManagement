import { RouterModule, Routes } from '@angular/router';
import { ListBpmnInstancesComponent } from './list/list.component';

const routes: Routes = [
    { path: '', component: ListBpmnInstancesComponent },
    { path: ':id', loadChildren: './view/view.module#ViewBpmnInstanceModule' }
];

export const BpmnInstancesRoutes = RouterModule.forChild(routes);