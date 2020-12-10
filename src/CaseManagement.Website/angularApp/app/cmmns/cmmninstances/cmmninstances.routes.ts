import { RouterModule, Routes } from '@angular/router';
import { ViewCmmnPlanInstanceComponent } from './view/view.component';
import { ViewCasePlanEltInstanceComponent } from './view/viewelt.component';
import { ViewTransitionHistoriesComponent } from './view/viewtransitionhistories.component';

const routes: Routes = [
    {
        path: ':id', component: ViewCmmnPlanInstanceComponent, children: [
            {
                path: ':eltid', component: ViewCasePlanEltInstanceComponent, children: [
                    { path: ':instid/history', component: ViewTransitionHistoriesComponent },
                ]
            }
        ]
    }
];

export const CmmnPlansRoutes = RouterModule.forChild(routes);