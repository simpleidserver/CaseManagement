import { RouterModule, Routes } from '@angular/router';
import { ListBpmnFilesComponent } from './list/list.component';
import { ViewBpmnFileComponent } from './view/view.component';

const routes: Routes = [
    { path: '', component: ListBpmnFilesComponent },
    { path: ':id', component: ViewBpmnFileComponent }
];

export const BpmnFilesRoutes = RouterModule.forChild(routes);