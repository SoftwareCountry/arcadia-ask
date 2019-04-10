import { Component, ChangeDetectionStrategy } from '@angular/core';
import { QuestionsStore } from '../questions-store.service';
import { MatDialog } from '@angular/material';
import { QuestionCreatedPopupComponent } from './question-created-popup/question-created-popup.component';

@Component({
  selector: 'app-add-question-form',
  templateUrl: './add-question-form.component.html',
  styleUrls: ['./add-question-form.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddQuestionFormComponent {
  public model = {
    text: ''
  };

  constructor(
    private readonly questionsStore: QuestionsStore,
    private readonly dialog: MatDialog,
  ) { }

  public onSubmit() {
    this.questionsStore.createQuestion(this.model.text)
      .then(() => this.openDialog())
      .then(() => this.model.text = '')
      .catch(x => console.error(x));
  }

  private openDialog(): void {
    this.dialog.open(
      QuestionCreatedPopupComponent, {
        data: {questionText: this.model.text}
      }
    );
  }
}
