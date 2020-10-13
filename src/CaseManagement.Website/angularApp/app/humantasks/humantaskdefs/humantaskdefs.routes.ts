import { RouterModule, Routes } from '@angular/router';
import { ViewHumanTaskDef } from './view/view.component';
import { ListHumanTaskDef } from './list/list.component';


const routes: Routes = [
    { path: ':id', component: ViewHumanTaskDef, loadChildren: './view/view.module#HumanTaskDefsViewModule' },
    { path: '', component: ListHumanTaskDef }
];

export const HumanTaskDefsRoutes = RouterModule.forChild(routes);