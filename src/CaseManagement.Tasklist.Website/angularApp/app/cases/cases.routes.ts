import { RouterModule, Routes } from '@angular/router';
import { ListCasesComponent } from './list/list.component';
import { ViewCaseComponent } from './view/view.component';


const routes: Routes = [
    { path: '', component: ListCasesComponent },
    { path: ':id', component: ViewCaseComponent }
];

export const CaseRoutes = RouterModule.forChild(routes);