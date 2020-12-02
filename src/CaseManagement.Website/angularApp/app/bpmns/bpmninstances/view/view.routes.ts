import { RouterModule, Routes } from '@angular/router';
import { ActivityStatesComponent } from './activitystates/activitystates.component';
import { IncomingTokensComponent } from './incomingtokens/incomingtokens.component';
import { OutgoingTokensComponent } from './outgoingtokens/outgoingtokens.component';
import { ViewBpmnInstanceComponent } from './view.component';
import { ViewExecutionPathComponent } from './viewexecutionpath.component';
import { ViewExecutionPointerComponent } from './viewpointer.component';

const routes: Routes = [
    {
        path: '', component: ViewBpmnInstanceComponent, children: [
            {
                path: ':pathid', component: ViewExecutionPathComponent, children: [
                    {
                        path: ':eltid', component: ViewExecutionPointerComponent, children: [
                            { path: '', redirectTo: 'incomingtokens', pathMatch: 'full' },
                            { path: 'activitystates', component: ActivityStatesComponent },
                            { path: 'incomingtokens', component: IncomingTokensComponent },
                            { path: 'outgoingtokens', component: OutgoingTokensComponent }
                        ]
                    }
                ]
            }
        ]
    }
];

export const ViewBpmnInstanceRoutes = RouterModule.forChild(routes);