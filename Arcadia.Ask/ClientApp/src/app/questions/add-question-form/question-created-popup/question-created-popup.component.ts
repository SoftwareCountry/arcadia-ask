import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { QuestionCreatedData } from './question-created-data';

@Component({
  selector: 'app-question-created-popup',
  templateUrl: 'question-created-popup.component.html',
  styleUrls: ['question-created-popup.component.html'],
})
export class QuestionCreatedPopupComponent {
  constructor(
    public dialogRef: MatDialogRef<QuestionCreatedPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: QuestionCreatedData
  ) {}

  public onOkClick(): void {
    this.dialogRef.close();
  }
}
