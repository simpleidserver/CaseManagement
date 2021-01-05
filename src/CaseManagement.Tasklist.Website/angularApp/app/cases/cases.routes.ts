import { RouterModule, Routes } from '@angular/router';
import { ListCasesComponent } from './list/list.component';
import { ViewCaseComponent } from './view/view.component';
import { ViewFormComponent } from './view/viewform.component';


const routes: Routes = [
    { path: '', component: ListCasesComponent },
    {
        path: ':id', component: ViewCaseComponent, children: [
            { path: ':formid', component: ViewFormComponent }
        ]
    }
];

export const CaseRoutes = RouterModule.forChild(routes);