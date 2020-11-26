import { RouterModule, Routes } from '@angular/router';
import { BpmnsComponent } from './bpmns.component';

const routes: Routes = [
    { path: '', redirectTo: 'bpmnfiles', pathMatch: 'full' },
    { path: 'bpmnfiles', component: BpmnsComponent, loadChildren: './bpmnfiles/bpmnfiles.module#BpmnFilesModule' },
    { path: 'bpmninstances', component: BpmnsComponent, loadChildren: './bpmninstances/bpmninstances.module#BpmnInstancesModule' }
];

export const BpmnsRoutes = RouterModule.forChild(routes);