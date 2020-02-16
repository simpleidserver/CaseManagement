import { RouterModule, Routes } from '@angular/router';
import { ListCasePlansComponent } from './list/list.component';
import { ViewCaseDefinitionComponent } from './view/view.component';


const routes: Routes = [
    { path: '', component: ListCasePlansComponent },
    { path: ':id', component: ViewCaseDefinitionComponent }
];

export const CasePlansRoutes = RouterModule.forChild(routes);