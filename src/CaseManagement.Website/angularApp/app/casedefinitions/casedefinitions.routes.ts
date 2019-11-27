import { RouterModule, Routes } from '@angular/router';

import { ListCaseDefsComponent } from './list-case-defs/list-case-defs.component';
import { CaseDefComponent } from './case-def/case-def.component';

const routes: Routes = [
    { path: '', component: ListCaseDefsComponent },
    { path: ':id', component: CaseDefComponent }
];

export const CaseDefinitionsRoutes = RouterModule.forChild(routes);