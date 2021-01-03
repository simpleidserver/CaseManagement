import { RouterModule, Routes } from '@angular/router';
import { ListCasesComponent } from './list/list.component';


const routes: Routes = [
    { path: '', component: ListCasesComponent }
];

export const CaseRoutes = RouterModule.forChild(routes);