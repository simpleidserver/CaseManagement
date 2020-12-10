import { RouterModule } from '@angular/router';
var routes = [
    { path: '', redirectTo: 'cmmnfiles', pathMatch: 'full' },
    { path: 'cmmnfiles', loadChildren: './cmmnfiles/cmmnfiles.module#CmmnFilesModule' },
    { path: 'cmmnplans', loadChildren: './cmmnplans/cmmnplans.module#CmmnPlansModule' }
];
export var CmmnsRoutes = RouterModule.forChild(routes);
//# sourceMappingURL=cmmns.routes.js.map