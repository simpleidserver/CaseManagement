import { RouterModule, Routes } from '@angular/router';
import { ListCaseDefinitionsComponent } from './list/list.component';
// import { ViewCaseDefinitionComponent } from './view/view.component';

const routes: Routes = [
    { path: '', component: ListCaseDefinitionsComponent },
    // { path: ':id', component: ViewCaseDefinitionComponent }
];

export const CaseDefinitionsRoutes = RouterModule.forChild(routes);