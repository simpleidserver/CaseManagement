import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store, ScannedActionsSubject } from '@ngrx/store';
import { PresentationElement } from '@app/stores/common/presentationelement.model';
import { filter } from 'rxjs/operators';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { TranslateService } from '@ngx-translate/core';
import { MatSnackBar } from '@angular/material';
import { PresentationParameter } from '@app/stores/common/presentationparameter.model';

@Component({
    selector: 'view-presentationparameters-component',
    templateUrl: './view-presentationparameters.component.html',
    styleUrls: ['./view-presentationparameters.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewPresentationParametersComponent implements OnInit {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW.PRESENTATION_PARAMETERS";
    humanTaskDef: HumanTaskDef;
    names: PresentationElement[] = [];
    subjects: PresentationElement[] = [];
    descriptions: PresentationElement[] = [];
    presentationParameters: PresentationParameter[] = [];

    constructor(
        private store: Store<fromAppState.AppState>,
        private actions$: ScannedActionsSubject,
        private translateService: TranslateService,
        private snackBar: MatSnackBar) {
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.COMPLETE_UPDATE_PRESENTATIONELEMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.PRESENTATIONPARAMETERS_UPDATED'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromHumanTaskDefActions.ActionTypes.ERROR_UPDATE_PRESENTATIONELEMENT))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('HUMANTASK.MESSAGES.ERROR_UPDATE_PRESENTATION_PARAMETERS'), this.translateService.instant('undo'), {
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
            this.names = HumanTaskDef.getNames(e);
            this.subjects = HumanTaskDef.getSubjects(e);
            this.descriptions = HumanTaskDef.getDescriptions(e);
            this.presentationParameters = e.presentationParameters;
        });
    }

    update() {
        let presentationElts: PresentationElement[] = [];
        presentationElts = presentationElts.concat(
            this.names,
            this.subjects,
            this.descriptions);
        const request = new fromHumanTaskDefActions.UpdatePresentationElementOperation(this.humanTaskDef.id, presentationElts, this.presentationParameters);
        this.store.dispatch(request);
    }
}