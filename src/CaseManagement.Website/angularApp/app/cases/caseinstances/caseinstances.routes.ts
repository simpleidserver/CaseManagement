import { RouterModule, Routes } from '@angular/router';
// import { ListCaseInstancesComponent } from './list/list.component';
// import { ViewCaseInstanceComponent } from './view/view.component';

const routes: Routes = [
    /*
    { path: '', component: ListCaseInstancesComponent },
    { path: ':id', component: ViewCaseInstanceComponent }
    */
];

export const CaseInstancesRoutes = RouterModule.forChild(routes);