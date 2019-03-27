import { Component, ChangeDetectionStrategy } from '@angular/core';
import { QuestionsStore } from '../questions-store.service';

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

  constructor(private readonly questionsStore: QuestionsStore) { }

  public onSubmit() {
    this.questionsStore.createQuestion(this.model.text).catch(x => console.error(x));
  }
}
