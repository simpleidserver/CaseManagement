import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: 'cmmnfiles', pathMatch: 'full' },
    { path: 'cmmnfiles', loadChildren: './cmmnfiles/cmmnfiles.module#CmmnFilesModule' },
    { path: 'cmmnplans', loadChildren: './cmmnplans/cmmnplans.module#CmmnPlansModule' },
    { path: 'cmmninstances', loadChildren: './cmmninstances/cmmninstances.module#CmmnInstancesModule' }
];

export const CmmnsRoutes = RouterModule.forChild(routes);