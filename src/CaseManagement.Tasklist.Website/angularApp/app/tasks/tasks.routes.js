import { RouterModule } from '@angular/router';
import { ListTasksComponent } from './list/list.component';
import { ViewTaskComponent } from './view/view.component';
var routes = [
    { path: '', component: ListTasksComponent },
    { path: ':id', component: ViewTaskComponent }
];
export var HomeRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=tasks.routes.js.map