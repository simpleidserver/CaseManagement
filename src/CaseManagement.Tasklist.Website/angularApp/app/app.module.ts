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
import { appReducer } from './stores/appstate';
import { CasesEffects } from './stores/cases/effects/cases.effects';
import { CasesService } from './stores/cases/services/cases.service';
import { NotificationsEffects } from './stores/notifications/effects/notifications.effects';
import { NotificationsService } from './stores/notifications/services/notifications.service';
import { TasksEffects } from './stores/tasks/effects/tasks.effects';
import { TasksService } from './stores/tasks/services/tasks.service';

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
        EffectsModule.forRoot([TasksEffects, NotificationsEffects, CasesEffects]),
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
    providers: [AuthGuard, TasksService, NotificationsService, CasesService]
})
export class AppModule { }