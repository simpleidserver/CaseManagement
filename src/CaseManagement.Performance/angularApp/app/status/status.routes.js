import { RouterModule } from '@angular/router';
import { UnauthorizedComponent } from './components/401/401.component';
import { NotFoundComponent } from './components/404/404.component';
var routes = [
    { path: '404', component: NotFoundComponent },
    { path: '401', component: UnauthorizedComponent }
];
export var StatusRoute = RouterModule.forChild(routes);
//# sourceMappingURL=status.routes.js.map