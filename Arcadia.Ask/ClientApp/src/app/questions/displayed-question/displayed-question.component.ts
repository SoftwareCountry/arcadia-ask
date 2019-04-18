import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { Question } from '../question';
import { QuestionsStore } from '../questions-store.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-displayed-question',
  templateUrl: './displayed-question.component.html',
  styleUrls: ['./displayed-question.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DisplayedQuestionComponent {
  @Input()
  public readonly editable: boolean;

  @Input()
  public readonly votingAvailable: boolean;

  public question$: Observable<Question>;

  constructor(private readonly questionsStore: QuestionsStore) {
    this.question$ = questionsStore.questions.pipe(
      map(question => question.first())
    );
  }

  public async vote(questionId: string, upvoted: boolean) {
    if (upvoted) {
      await this.questionsStore.upvoteQuestion(questionId);
    } else {
      await this.questionsStore.downvoteQuestion(questionId);
    }
  }

  public async delete(questionId: string) {
    await this.questionsStore.removeQuestion(questionId);
  }

  public async approve(questionId: string) {
    await this.questionsStore.approveQuestion(questionId);
  }

}
