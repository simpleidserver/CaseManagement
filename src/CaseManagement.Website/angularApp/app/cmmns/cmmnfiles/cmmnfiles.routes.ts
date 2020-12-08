import { RouterModule, Routes } from '@angular/router';
import { ListCmmnFilesComponent } from './list/list.component';
import { ViewCmmnFileInformationComponent } from './view/information/information.component';
import { ViewCmmnFileUIEditorComponent } from './view/uieditor/uieditor.component';
import { ViewCmmnFileComponent } from './view/view.component';
import { ViewCmmnFileXmlEditorComponent } from './view/xmleditor/xmleditor.component';
import { ListCmmnPlansComponent } from './view/plans/plans.component';


const routes: Routes = [
    { path: '', component: ListCmmnFilesComponent },
    {
        path: ':id', component: ViewCmmnFileComponent, children: [
            { path: '', redirectTo: 'info', pathMatch: 'full' },
            { path: 'info', component: ViewCmmnFileInformationComponent },
            { path: 'editor', component: ViewCmmnFileUIEditorComponent },
            { path: 'xml', component: ViewCmmnFileXmlEditorComponent },
            { path: 'plans', component: ListCmmnPlansComponent }
        ]
    }
];

export const CmmnFilesRoutes = RouterModule.forChild(routes);