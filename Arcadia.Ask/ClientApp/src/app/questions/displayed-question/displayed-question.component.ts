import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Observable } from 'rxjs';

import { DisplayedQuestionService } from '../displayed-question.service';
import { Question } from '../question';
import { QuestionsStore } from '../questions-store.service';

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

  @Input()
  public readonly hidingAvailable: boolean;

  public question$: Observable<Question>;

  constructor(
    private readonly questionsStore: QuestionsStore,
    private readonly displayedQuestionService: DisplayedQuestionService
  ) {
    this.question$ = this.displayedQuestionService.displayedQuestion;
  }

  public async hide() {
    await this.displayedQuestionService.hideQuestion();
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
