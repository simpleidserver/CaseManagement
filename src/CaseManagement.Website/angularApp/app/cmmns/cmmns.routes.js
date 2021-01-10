import { RouterModule } from '@angular/router';
import { CmmnsComponent } from './cmmns.component';
import { ViewCmmnFileComponent } from './viewfile/viewfile.component';
import { ViewCmmnInstanceComponent } from './viewinstance/viewinstance.component';
var routes = [
    { path: '', component: CmmnsComponent },
    { path: ':id', component: ViewCmmnFileComponent },
    { path: ':id/:instanceid', component: ViewCmmnInstanceComponent }
];
export var CmmnsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=cmmns.routes.js.map