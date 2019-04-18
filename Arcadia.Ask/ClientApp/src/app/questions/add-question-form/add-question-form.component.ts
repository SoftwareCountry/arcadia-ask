import { Component, ChangeDetectionStrategy, ViewChild } from '@angular/core';
import { QuestionsStore } from '../questions-store.service';
import { MatDialog } from '@angular/material';
import { QuestionCreatedPopupComponent, QuestionCreatedData } from './question-created-popup';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-add-question-form',
  templateUrl: './add-question-form.component.html',
  styleUrls: ['./add-question-form.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddQuestionFormComponent {
  @ViewChild('questionForm')
  public questionForm: NgForm;

  public model = {
    text: ''
  };

  constructor(
    private readonly questionsStore: QuestionsStore,
    private readonly dialog: MatDialog,
  ) { }

  public onSubmit() {
    this.questionsStore.createQuestion(this.model.text)
      .then(() => {
        this.notifyAboutQuestionCreated();
        this.questionForm.resetForm();
      })
      .catch(x => console.error(x));
  }

  private notifyAboutQuestionCreated(): void {
    const data: QuestionCreatedData = { questionText: this.model.text };
    this.dialog.open<QuestionCreatedPopupComponent, QuestionCreatedData>(
      QuestionCreatedPopupComponent, { data }
    );
  }
}
