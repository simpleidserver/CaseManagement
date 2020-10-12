import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import * as fromAppState from '@app/stores/appstate';
import { HumanTaskDef } from '@app/stores/humantaskdefs/models/humantaskdef.model';
import { select, Store } from '@ngrx/store';
import { PresentationElement } from '@app/stores/common/presentationelement.model';
@Component({
    selector: 'view-presentationparameters-component',
    templateUrl: './view-presentationparameters.component.html',
    styleUrls: ['./view-presentationparameters.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewPresentationParametersComponent implements OnInit {
    presentationElt: PresentationElement;

    constructor(
        private store: Store<fromAppState.AppState>) {
    }

    ngOnInit(): void {
        this.store.pipe(select(fromAppState.selectHumanTaskResult)).subscribe((e: HumanTaskDef) => {
            if (!e) {
                return;
            }

            this.presentationElt = e.presentationElement;
        });
    }

    update() {
        // UPDATE THE PRESENTATION PARAMETER !!!
    }
}
