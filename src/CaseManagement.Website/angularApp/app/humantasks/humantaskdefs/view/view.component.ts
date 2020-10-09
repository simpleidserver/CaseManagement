import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as fromAppState from '@app/stores/appstate';
import * as fromHumanTaskDefActions from '@app/stores/humantaskdefs/actions/humantaskdef.actions';
import { Store } from '@ngrx/store';

@Component({
    selector: 'view-humantaskdef-component',
    templateUrl: './view.component.html',
    styleUrls: ['./view.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ViewHumanTaskDef implements OnInit {
    baseTranslationKey: string = "HUMANTASK.DEF.VIEW";

    constructor(
        private store: Store<fromAppState.AppState>,
        private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.refresh();
    }

    refresh() {
        const id = this.route.snapshot.params['id'];
        const request = new fromHumanTaskDefActions.GetHumanTaskDef(id);
        this.store.dispatch(request);
    }
}
