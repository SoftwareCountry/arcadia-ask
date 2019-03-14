import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { Question } from '../question';
import { QuestionsStore } from '../questions-store.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-questions-list',
  templateUrl: './questions-list.component.html',
  styleUrls: ['./questions-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class QuestionsListComponent implements OnInit {

  public readonly questions: Observable<Question[]>;

  constructor(questionsStore: QuestionsStore) {
    this.questions = questionsStore
      .questions
      .pipe(map(x => x.valueSeq().toArray()));
  }

  ngOnInit() {
  }

  questionIdTrack(x: Question) {
    return x.questionId;
  }

}
