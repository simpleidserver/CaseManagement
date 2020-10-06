import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../infrastructure/auth-guard.service';
import { HumanTasksComponent } from './humantasks.component';

const routes: Routes = [
    { path: '', redirectTo: 'humantaskdefs', pathMatch: 'full' },
    { path: 'humantaskdefs', component: HumanTasksComponent, loadChildren: './humantaskdefs/humantaskdefs.module#HumanTaskDefsModule', canActivate: [AuthGuard], data: { role: ['businessanalyst'] }  },
];

export const HumanTasksRoutes = RouterModule.forChild(routes);