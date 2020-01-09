import { RouterModule, Routes } from '@angular/router';

import { ListCaseFilesComponent } from './list-case-files/list-case-files.component';
import { ViewCaseFileComponent } from './view-case-file/view-case-file.component';
import { ViewCaseDefinitionComponent } from './view-case-definition/view-case-definition.component';

const routes: Routes = [
    { path: '', component: ListCaseFilesComponent },
    { path: ':id', component: ViewCaseFileComponent },
    { path: ':id/:defid', component: ViewCaseDefinitionComponent }
];

export const CaseFilesRoutes = RouterModule.forChild(routes);