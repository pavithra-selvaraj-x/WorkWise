import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';

export interface ConfirmationDialogData {
  title?: string;
  message?: string;
  cancelButtonText?: string;
  okButtonText?: string;
}

@Component({
  selector: 'ww-confirmation-dialog',
  standalone: true,
  imports: [MatDialogModule, CommonModule, MatButtonModule],
  templateUrl: './confirmation-dialog.component.html',
  styleUrl: './confirmation-dialog.component.scss',
})
export class ConfirmationDialogComponent {
  title: string = 'Confirm?';
  message: string = 'Are you sure you want to proceed?';
  cancelButtonText: string = 'Dismiss';
  okButtonText: string = 'Confirm';

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: ConfirmationDialogData,
    public dialogRef: MatDialogRef<ConfirmationDialogComponent>
  ) {
    if (data.title) {
      this.title = data.title;
    }
    if (data.message) {
      this.message = data.message;
    }
    if (data.cancelButtonText) {
      this.cancelButtonText = data.cancelButtonText;
    }
    if (data.okButtonText) {
      this.okButtonText = data.okButtonText;
    }
  }

  clickOkButton() {
    this.dialogRef.close(true);
  }
}
