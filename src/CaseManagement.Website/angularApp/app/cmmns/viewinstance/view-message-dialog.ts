import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
    selector: 'view-message-dialog',
    templateUrl: 'view-message-dialog.html',
})
export class ViewMessageDialog {
    json: string;
    editorOptions: any = { theme: 'vs-dark', language: 'json', automaticLayout: true };

    constructor(
        public dialogRef: MatDialogRef<ViewMessageDialog>,
        @Inject(MAT_DIALOG_DATA) public data: { json: string }) {
        this.json = data.json;
    }
}
