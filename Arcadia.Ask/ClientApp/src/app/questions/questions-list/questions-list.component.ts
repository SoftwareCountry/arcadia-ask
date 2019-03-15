import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { Question } from '../question';
import { QuestionsStore } from '../questions-store.service';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { async } from '@angular/core/testing';

@Component({
  selector: 'app-questions-list',
  templateUrl: './questions-list.component.html',
  styleUrls: ['./questions-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class QuestionsListComponent implements OnInit {

  public readonly questions: Observable<Question[]>;

  constructor(private readonly questionsStore: QuestionsStore) {
    this.questions = this.questionsStore
      .questions
      .pipe(
        map(x => x.valueSeq().sortBy(q => -q.votes).toArray()),
        tap(console.log)
      );
  }

  ngOnInit() {
  }

  questionIdTrack(index: number, x: Question) {
    return x.questionId;
  }

  async onVoted(questionId: string, upvoted: boolean) {
    if (upvoted) {
      await this.questionsStore.upvoteQuestion(questionId);
    } else {
      await this.questionsStore.downvoteQuestion(questionId);
    }
  }

}
