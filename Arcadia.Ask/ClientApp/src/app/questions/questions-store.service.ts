import { Injectable, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Map } from 'immutable';
import { ConnectableObservable, from, Observable, ReplaySubject, Subscription } from 'rxjs';
import { flatMap, multicast, scan, startWith, switchMap } from 'rxjs/operators';

import { Question, QuestionMetadata } from './question';

type QuestionChange =
  { type: 'changed', question: QuestionMetadata } |
  { type: 'removed', id: string } |
  { type: 'voted', id: string };

@Injectable({
  providedIn: 'root'
})
export class QuestionsStore implements OnDestroy {
  public questions: Observable<Map<string, Question>>;

  private readonly hubConnection: Promise<HubConnection>;

  private readonly questionsSubscription: Subscription;

  constructor() {
    this.hubConnection = this.connect();
    this.questions = from(this.hubConnection).pipe(
      flatMap(x => this.getQuestionsStream(x)),
      multicast(new ReplaySubject(1))
    );
    this.questionsSubscription = (this.questions as ConnectableObservable<Map<string, Question>>).connect();
  }

  public createQuestion(text: string) {
    return this.invoke('CreateQuestion', text);
  }

  public approveQuestion(questionId: string) {
    return this.invoke('ApproveQuestion', questionId);
  }

  public upvoteQuestion(questionId: string) {
    return this.invoke('UpvoteQuestion', questionId);
  }

  public downvoteQuestion(questionId: string) {
    return this.invoke('DownvoteQuestion', questionId);
  }

  public removeQuestion(questionId: string) {
    return this.invoke('RemoveQuestion', questionId);
  }

  public ngOnDestroy() {
    this.dispose().catch((x) => console.error(x));
  }

  private getQuestionsStream(hubConnection: HubConnection) {
    const initialArray = from(this.getInitialArray());
    return initialArray.pipe(
      switchMap(seed => {
        return this.onQuestionChanges(hubConnection).pipe(
          scan((acc, value: QuestionChange) => this.applyQuestionsChange(acc, value), seed),
          startWith(seed)
        );
      })
    );
  }

  private async connect() {
    const url = '/questions';
    const connection = new HubConnectionBuilder().withUrl(url).build();

    await connection.start();
    return connection;
  }

  private async dispose() {
    try {
      await (await this.hubConnection).stop();
    } finally {
      this.questionsSubscription.unsubscribe();
    }
  }

  private async getQuestions() {
    return this.invoke<Question[]>('GetQuestions');
  }

  private async getInitialArray() {
    const data = await this.getQuestions();
    const mappedData = data.map<[string, Question]>(x => [ x.metadata.questionId, x ]);
    return Map(mappedData);
  }

  private async invoke<T>(methodName: string, ...args: any[]) {
    const connection = await this.hubConnection;
    return connection.invoke<T>(methodName, ...args);
  }

  private onQuestionChanges(hubConnection: HubConnection) {
    return new Observable<QuestionChange>(subscriber => {

      const onQuestionChanged = (question: QuestionMetadata) => {
        subscriber.next({ type: 'changed', question });
      };

      const onQuestionRemoved = (id: string) => {
        subscriber.next({ type: 'removed', id });
      };

      const onQuestionVoted = (id: string) => {
        subscriber.next({ type: 'voted', id });
      };

      hubConnection.on('QuestionIsChanged', onQuestionChanged);
      hubConnection.on('QuestionIsRemoved', onQuestionRemoved);
      hubConnection.on('QuestionIsVoted', onQuestionVoted);

      hubConnection.onclose(error => {
        if (error) {
          subscriber.error(error);
        } else {
          subscriber.complete();
        }
      });

      return () => {
        hubConnection.off('QuestionIsChanged', onQuestionChanged);
        hubConnection.off('QuestionIsRemoved', onQuestionRemoved);
        hubConnection.off('QuestionIsVoted', onQuestionVoted);
      };
    });
  }

  private applyQuestionsChange(acc: Map<string, Question>, change: QuestionChange): Map<string, Question> {
    switch (change.type) {
      case 'changed':
        return this.questionChanged(acc, change.question);

      case 'removed':
        return acc.remove(change.id);

      case 'voted':
        const oldQuestion = acc.get(change.id);
        return acc.set(change.id, {
          metadata: oldQuestion.metadata,
          didVote: true
        });

      default:
        console.error('Unknown change type');
    }
  }

  private questionChanged(acc: Map<string, Question>, question: QuestionMetadata): Map<string, Question> {
    const oldQuestion = acc.get(question.questionId);
    const didVote =
      oldQuestion !== null &&
      oldQuestion !== undefined &&
      oldQuestion.didVote;

    return acc.set(question.questionId, {
      metadata: question,
      didVote: didVote
    });
  }
}
