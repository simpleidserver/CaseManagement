var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { OAuthModule } from 'angular-oauth2-oidc';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { HomeModule } from './home/home.module';
import { AuthGuard } from './infrastructure/auth-guard.service';
import { MaterialModule } from './shared/material.module';
import { SharedModule } from './shared/shared.module';
export function createTranslateLoader(http) {
    var url = process.env.BASE_URL + 'assets/i18n/';
    return new TranslateHttpLoader(http, url, '.json');
}
var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        NgModule({
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
            providers: [AuthGuard]
        })
    ], AppModule);
    return AppModule;
}());
export { AppModule };
//# sourceMappingURL=app.module.js.map