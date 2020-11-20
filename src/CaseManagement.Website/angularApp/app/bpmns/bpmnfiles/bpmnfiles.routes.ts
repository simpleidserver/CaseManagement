import { RouterModule, Routes } from '@angular/router';
import { ListBpmnFilesComponent } from './list/list.component';

const routes: Routes = [
    { path: '', component: ListBpmnFilesComponent }
];

export const BpmnFilesRoutes = RouterModule.forChild(routes);