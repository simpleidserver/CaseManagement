import { Component, Inject } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { BaseUIComponent } from "../baseui.component";

@Component({
    selector: 'view-header',
    templateUrl: 'header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent extends BaseUIComponent {
    constructor(private dialog: MatDialog) {
        super();
    }

    openDialog() {
        const dialogRef = this.dialog.open(HeaderComponentDialog, {
            data: { opt: this.option }
        });
        dialogRef.afterClosed().subscribe((r: any) => {
            if (!r) {
                return;
            }
        });
    }
}

@Component({
    selector: 'view-header-dialog',
    templateUrl: 'headerdialog.component.html',
})
export class HeaderComponentDialog {
    configureHeaderForm: FormGroup;
    constructor(
        @Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<HeaderComponentDialog>
    ) {
        this.configureHeaderForm = new FormGroup({
            txt: new FormControl({ value: '' }),
            class: new FormControl({ value: '' })
        });
        this.configureHeaderForm.get('txt').setValue(data.opt.txt);
        this.configureHeaderForm.get('class').setValue(data.opt.class);
    }

    onSave(val: { txt: string, class: string }) {
        const opt = this.data.opt;
        opt.txt = val.txt;
        opt.class = val.class;
        this.dialogRef.close(opt);
    }
}