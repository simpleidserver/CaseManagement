import { RouterModule, Routes } from '@angular/router';
import { CaseInstanceComponent } from './case-instance/case-instance.component';

const routes: Routes = [
    { path: ':id', component: CaseInstanceComponent }
];

export const CaseInstancesRoutes = RouterModule.forChild(routes);