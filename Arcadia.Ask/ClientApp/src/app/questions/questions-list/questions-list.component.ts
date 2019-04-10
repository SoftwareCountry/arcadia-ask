import { Component, ChangeDetectionStrategy, Input } from '@angular/core';
import { Question } from '../question';
import { QuestionsStore } from '../questions-store.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Map } from 'immutable';

@Component({
  selector: 'app-questions-list',
  templateUrl: './questions-list.component.html',
  styleUrls: ['./questions-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class QuestionsListComponent {

  @Input()
  public readonly editable: boolean;

  @Input()
  public readonly withVoteButton: boolean;

  public readonly questions: Observable<Question[]>;

  constructor(private readonly questionsStore: QuestionsStore) {
    this.questions = this.questionsStore
      .questions
      .pipe(
        map(x => this.extractQuestions(x))
      );
  }

  public questionIdTrack(index: number, x: Question) {
    return x.metadata.questionId;
  }

  public async onVoted(questionId: string, upvoted: boolean) {
    if (upvoted) {
      await this.questionsStore.upvoteQuestion(questionId);
    } else {
      await this.questionsStore.downvoteQuestion(questionId);
    }
  }

  public async onDeleted(questionId: string) {
    await this.questionsStore.removeQuestion(questionId);
  }

  public async onApproved(questionId: string) {
    await this.questionsStore.approveQuestion(questionId);
  }

  private extractQuestions(source: Map<string, Question>) {
    let seq = source.valueSeq();
    if (!this.editable) {
      seq = seq.filter(x => x.metadata.isApproved);
    }

    return seq.sort((a, b) => {
      if (a.metadata.isApproved) {
        return -1;
      }

      return b.metadata.votes - a.metadata.votes;
    }).toArray();
  }
}
