import { RouterModule, Routes } from '@angular/router';
import { ListCaseFilesComponent } from './list/list.component';
import { ViewCaseFilesComponent } from './view/view.component';
import { HistoryCaseFileComponent } from './history/history.component';


const routes: Routes = [
    { path: '', component: ListCaseFilesComponent },
    { path: ':id', component: ViewCaseFilesComponent },
    { path: ':id/history', component: HistoryCaseFileComponent }
];

export const CaseFilesRoutes = RouterModule.forChild(routes);