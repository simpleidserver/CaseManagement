import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromDelegateConf from '@app/stores/delegateconfigurations/actions/delegateconfiguration.actions';
import { GetDelegateConfiguration, UpdateDelegateConfiguration } from '@app/stores/delegateconfigurations/actions/delegateconfiguration.actions';
import { DelegateConfiguration } from '@app/stores/delegateconfigurations/models/delegateconfiguration.model';
import { ScannedActionsSubject, select, Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { filter } from 'rxjs/operators';

@Component({
    selector: 'view-delegate',
    templateUrl: './viewdelegate.component.html',
    styleUrls: ['./viewdelegate.component.scss']
})
export class ViewDelegateConfigurationComponent implements OnInit, OnDestroy {
    listener: any;
    content$: DelegateConfiguration;
    records: { key: string, value: string }[] = [];
    updateParametersFormGroup: FormGroup;

    constructor(
        private activatedRoute: ActivatedRoute,
        private store: Store<fromAppState.AppState>,
        private actions$: ScannedActionsSubject,
        private snackBar: MatSnackBar,
        private translateService: TranslateService) {
    }

    ngOnInit() {
        this.updateParametersFormGroup = new FormGroup({});
        this.actions$.pipe(
            filter((action: any) => action.type === fromDelegateConf.ActionTypes.ERROR_UPDATE_DELEGATE_CONFIGURATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('DELEGATECONFIGURATION.MESSAGES.ERROR_UPDATE_DELEGATE_CONFIGURATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.actions$.pipe(
            filter((action: any) => action.type === fromDelegateConf.ActionTypes.COMPLETE_UPDATE_DELEGATE_CONFIGURATION))
            .subscribe(() => {
                this.snackBar.open(this.translateService.instant('DELEGATECONFIGURATION.MESSAGES.COMPLETE_UPDATE_DELEGATE_CONFIGURATION'), this.translateService.instant('undo'), {
                    duration: 2000
                });
            });
        this.listener = this.store.pipe(select(fromAppState.selectDelegateConfigurationResult)).subscribe((delegateConfiguration: DelegateConfiguration) => {
            if (!delegateConfiguration) {
                return;
            }

            this.content$ = delegateConfiguration;
            if (delegateConfiguration.records) {
                for (const k in delegateConfiguration.records) {
                    this.updateParametersFormGroup.addControl(k, new FormControl(delegateConfiguration.records[k]));
                    this.records.push({ key: k, value: delegateConfiguration.records[k] });
                }
            }
        });
        this.refresh();
    }

    ngOnDestroy(): void {
        if (this.listener) {
            this.listener.unsubscribe();
        }
    }

    onSave(data: any) {
        const id = this.activatedRoute.snapshot.params['id'];
        this.store.dispatch(new UpdateDelegateConfiguration(id, data));
    }

    private refresh() {
        const id = this.activatedRoute.snapshot.params['id'];
        this.store.dispatch(new GetDelegateConfiguration(id));
    }
}
