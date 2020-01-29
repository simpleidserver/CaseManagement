import { RouterModule, Routes } from '@angular/router';
import { ListCaseFilesComponent } from './list/list.component';
import { ViewCaseFilesComponent } from './view/view.component';


const routes: Routes = [
    { path: '', component: ListCaseFilesComponent },
    { path: ':id', component: ViewCaseFilesComponent }
];

export const CaseFilesRoutes = RouterModule.forChild(routes);