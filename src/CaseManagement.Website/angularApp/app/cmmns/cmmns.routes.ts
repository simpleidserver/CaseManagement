import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: 'cmmnfiles', pathMatch: 'full' },
    { path: 'cmmnfiles', loadChildren: './cmmnfiles/cmmnfiles.module#CmmnFilesModule' }
];

export const CmmnsRoutes = RouterModule.forChild(routes);