import { RouterModule } from '@angular/router';
import { ActivityStatesComponent } from './activitystates/activitystates.component';
import { IncomingTokensComponent } from './incomingtokens/incomingtokens.component';
import { OutgoingTokensComponent } from './outgoingtokens/outgoingtokens.component';
import { ViewBpmnInstanceComponent } from './view.component';
import { ViewExecutionPathComponent } from './viewexecutionpath.component';
import { ViewExecutionPointerComponent } from './viewpointer.component';
var routes = [
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
export var ViewBpmnInstanceRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=view.routes.js.map