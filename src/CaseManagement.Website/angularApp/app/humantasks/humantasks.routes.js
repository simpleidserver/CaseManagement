import { RouterModule } from '@angular/router';
import { AuthGuard } from '../infrastructure/auth-guard.service';
import { HumanTasksComponent } from './humantasks.component';
var routes = [
    { path: '', redirectTo: 'humantaskdefs', pathMatch: 'full' },
    { path: 'humantaskdefs', component: HumanTasksComponent, loadChildren: './humantaskdefs/humantaskdefs.module#HumanTaskDefsModule', canActivate: [AuthGuard], data: { role: ['businessanalyst'] } },
];
export var HumanTasksRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=humantasks.routes.js.map