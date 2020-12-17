import { RouterModule, Routes } from '@angular/router';
import { CmmnsComponent } from './cmmns.component';
import { ViewCmmnFileComponent } from './viewfile/viewfile.component';
import { ViewCmmnInstanceComponent } from './viewinstance/viewinstance.component';

const routes: Routes = [
    { path: '', component: CmmnsComponent },
    { path: ':id', component: ViewCmmnFileComponent },
    { path: ':id/:instanceid', component: ViewCmmnInstanceComponent }
];

export const CmmnsRoutes = RouterModule.forChild(routes);