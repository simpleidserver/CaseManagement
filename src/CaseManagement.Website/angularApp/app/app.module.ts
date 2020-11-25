import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { OAuthModule } from 'angular-oauth2-oidc';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { HomeModule } from './home/home.module';
import { AuthGuard } from './infrastructure/auth-guard.service';
import { MaterialModule } from './shared/material.module';
import { SharedModule } from './shared/shared.module';
import { SidenavService } from './shared/SidenavService';
import { appReducer } from './stores/appstate';
import { BpmnFilesEffects } from './stores/bpmnfiles/effects/bpmn-files.effects';
import { BpmnFilesService } from './stores/bpmnfiles/services/bpmnfiles.service';
import { BpmnInstancesEffects } from './stores/bpmninstances/effects/bpmn-instances.effects';
import { CaseFilesEffects } from './stores/casefiles/effects/case-files.effects';
import { CaseFilesService } from './stores/casefiles/services/casefiles.service';
import { CasePlanInstanceEffects } from './stores/caseplaninstances/effects/caseplaninstance.effects';
import { CasePlanInstanceService } from './stores/caseplaninstances/services/caseplaninstance.service';
import { CasePlanEffects } from './stores/caseplans/effects/caseplan.effects';
import { CasePlanService } from './stores/caseplans/services/caseplan.service';
import { HumanTaskDefEffects } from './stores/humantaskdefs/effects/humantaskdef.effects';
import { HumanTaskDefService } from './stores/humantaskdefs/services/humantaskdef.service';
import { HumanTaskInstEffects } from './stores/humantaskinstances/effects/humantaskinst.effects';
import { HumanTaskInstService } from './stores/humantaskinstances/services/humantaskinst.service';
import { BpmnInstancesService } from './stores/bpmninstances/services/bpmninstances.service';

export function createTranslateLoader(http: HttpClient) {
    const url = process.env.BASE_URL + 'assets/i18n/';
    return new TranslateHttpLoader(http, url, '.json');
}

@NgModule({
    imports: [
        RouterModule.forRoot(routes),
        SharedModule,
        MaterialModule,
        HomeModule,
        MatFormFieldModule,
        FlexLayoutModule,
        BrowserAnimationsModule,
        HttpClientModule,
        OAuthModule.forRoot(),
        EffectsModule.forRoot([BpmnInstancesEffects, CaseFilesEffects, CasePlanInstanceEffects, CasePlanEffects, HumanTaskDefEffects, HumanTaskInstEffects, BpmnFilesEffects]),
        StoreModule.forRoot(appReducer),
        StoreDevtoolsModule.instrument({
            maxAge: 10
        }),
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: (createTranslateLoader),
                deps: [HttpClient]
            }
        })
    ],
    declarations: [
        AppComponent
    ],
    bootstrap: [AppComponent],
    providers: [AuthGuard, CaseFilesService, CasePlanService, CasePlanInstanceService, HumanTaskDefService, HumanTaskInstService, BpmnFilesService, SidenavService, BpmnInstancesService ]
})
export class AppModule { }