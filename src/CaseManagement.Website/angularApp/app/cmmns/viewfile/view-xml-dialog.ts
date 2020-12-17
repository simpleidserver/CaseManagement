import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
    selector: 'view-xml-dialog',
    templateUrl: 'view-xml-dialog.html',
})
export class ViewXmlDialog {
    xml: string;
    editorOptions: any = { theme: 'vs-dark', language: 'xml', automaticLayout: true };

    constructor(
        public dialogRef: MatDialogRef<ViewXmlDialog>,
        @Inject(MAT_DIALOG_DATA) public data: { xml: string }) {
        this.xml = this.formatXML(data.xml);
    }

    update() {
        this.dialogRef.close({ xml: this.xml });
    }

    private formatXML(xml: any) {
        const PADDING = ' '.repeat(2);
        const reg = /(>)(<)(\/*)/g;
        let pad = 0;
        xml = xml.replace(reg, '$1\r\n$2$3');
        return xml.split('\r\n').map((node: any) => {
            let indent = 0;
            if (node.match(/.+<\/\w[^>]*>$/)) {
                indent = 0;
            } else if (node.match(/^<\/\w/) && pad > 0) {
                pad -= 1;
            } else if (node.match(/^<\w[^>]*[^\/]>.*$/)) {
                indent = 1;
            } else {
                indent = 0;
            }

            pad += indent;

            return PADDING.repeat(pad - indent) + node;
        }).join('\r\n');
    }
}
