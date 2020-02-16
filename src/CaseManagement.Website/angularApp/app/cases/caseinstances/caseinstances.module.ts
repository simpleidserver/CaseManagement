import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
// import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { MaterialModule } from '../../shared/material.module';
import { SharedModule } from '../../shared/shared.module';
// import { CaseDefinitionsEffects } from '../casedefinitions/effects/case-definitions';
// import { CaseInstancesEffects } from '../casedefinitions/effects/case-instances';
// import { CaseDefinitionsService } from '../casedefinitions/services/casedefinitions.service';
// import { CaseInstancesService } from '../casedefinitions/services/caseinstances.service';
// import { CaseFilesEffects } from '../casefiles/effects/case-files';
// import { CaseFilesService } from '../casefiles/services/casefiles.service';
import { CaseInstancesRoutes } from './caseinstances.routes';
import { ListCaseInstancesComponent } from './list/list.component';
import * as reducers from './reducers';
import { CaseElementInstanceDialog, ViewCaseInstanceComponent } from './view/view.component';

@NgModule({
    imports: [
        CommonModule,
        CaseInstancesRoutes,
        SharedModule,
        MaterialModule,
        // EffectsModule.forRoot([CaseDefinitionsEffects, CaseInstancesEffects, CaseFilesEffects]),
        StoreModule.forRoot(reducers.appReducer),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        })
    ],
    entryComponents: [ CaseElementInstanceDialog ],
    declarations: [ViewCaseInstanceComponent, ListCaseInstancesComponent, CaseElementInstanceDialog
    ],
    // providers: [ CaseInstancesService, CaseDefinitionsService, CaseFilesService ]
})

export class CaseInstancesModule { }