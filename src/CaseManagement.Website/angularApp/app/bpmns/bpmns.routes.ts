import { RouterModule, Routes } from '@angular/router';
import { BpmnsComponent } from './bpmns.component';

const routes: Routes = [
    { path: '', redirectTo: 'bpmnfiles', pathMatch: 'full' },
    { path: 'bpmnfiles', component: BpmnsComponent, loadChildren: './bpmnfiles/bpmnfiles.module#BpmnFilesModule'  }
];

export const BpmnsRoutes = RouterModule.forChild(routes);