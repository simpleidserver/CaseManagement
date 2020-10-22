import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { PresentationElement } from '@app/stores/common/presentationelement.model';
import { filter } from 'rxjs/operators';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material';

@Component({
    selector: 'view-presentationparameters-component',
    templateUrl: './view-presentationparameters.component.html',
    styleUrls: ['./view-presentationparameters.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewPresentationParametersComponent implements OnInit {
    presentationElt: PresentationElement = new PresentationElement();
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.PRESENTATION_PARAMETERS";
    humanTaskDef: HumanTaskDef;

    constructor(
        private store: Store<fromAppState.AppState>,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private snackBar: MatSnackBar) {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.PRESENTATIONPARAMETERS_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_PRESENTATIONELEMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant(this.baseTranslationKey + '.CANNOT_UPDATE_PRESENTATIONPARAMETERS'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.humanTaskDef = e;
            this.presentationElt = e.presentationElementResult;
        });
    }

    update() {
        const request = new fromHumanTaskDefActions.UpdatePresentationElementOperation(this.humanTaskDef.id, this.presentationElt);
        this.store.dispatch(request);
    }
}