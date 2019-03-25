import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { Question } from '../question';
import { QuestionsStore } from '../questions-store.service';

@Component({
  selector: 'app-add-question-form',
  templateUrl: './add-question-form.component.html',
  styleUrls: ['./add-question-form.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddQuestionFormComponent implements OnInit {
  public model = {
    text: ''
  };

  constructor(private readonly questionsStore: QuestionsStore) { }

  public ngOnInit() {
  }

  public onSubmit() {
    this.questionsStore.createQuestion(this.model.text).catch(x => console.error(x));
  }
}
