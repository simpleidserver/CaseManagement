import { RouterModule, Routes } from '@angular/router';
import { ViewHumanTaskDef } from './view/view.component';


const routes: Routes = [
    { path: ':id', component: ViewHumanTaskDef }
];

export const HumanTaskDefsRoutes = RouterModule.forChild(routes);