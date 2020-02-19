import { RouterModule, Routes } from '@angular/router';
import { ListCasePlansComponent } from './list/list.component';
import { ViewCaseDefinitionComponent } from './view/view.component';
import { HistoryCasePlanComponent } from './history/history.component';

const routes: Routes = [
    { path: '', component: ListCasePlansComponent },
    { path: ':id', component: ViewCaseDefinitionComponent },
    { path: ':id/history', component: HistoryCasePlanComponent }
];

export const CasePlansRoutes = RouterModule.forChild(routes);