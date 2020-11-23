import { RouterModule, Routes } from '@angular/router';
import { ListBpmnFilesComponent } from './list/list.component';
import { ViewBpmnFileInformationComponent } from './view/information/information.component';
import { ViewBpmnFileUIEditorComponent } from './view/uieditor/uieditor.component';
import { ViewBpmnFileXMLEditorComponent } from './view/xmleditor/xmleditor.component';
import { ViewBpmnFileComponent } from './view/view.component';

const routes: Routes = [
    { path: '', component: ListBpmnFilesComponent },
    {
        path: ':id', component: ViewBpmnFileComponent, children : [
            { path: '', redirectTo: 'info', pathMatch: 'full' },
            { path: 'info', component: ViewBpmnFileInformationComponent },
            { path: 'editor', component: ViewBpmnFileUIEditorComponent },
            { path: 'xml', component: ViewBpmnFileXMLEditorComponent }
        ]
    }
];

export const BpmnFilesRoutes = RouterModule.forChild(routes);