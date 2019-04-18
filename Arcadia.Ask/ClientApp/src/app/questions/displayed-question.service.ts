import { Injectable, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Observable, from, ReplaySubject, concat, Subscription, ConnectableObservable, combineLatest } from 'rxjs';
import { flatMap, multicast, map, withLatestFrom } from 'rxjs/operators';
import { QuestionsStore } from './questions-store.service';
import { Question } from './question';
import { Map } from 'immutable';

type DisplayedQuestionChange =
  { type: 'changed', newQuestionId: string } |
  { type: 'hidden' };

@Injectable({
  providedIn: 'root',
})
export class DisplayedQuestionService implements OnDestroy {
  public displayedQuestion: Observable<Question>;

  private readonly allQuestions: Observable<Map<string, Question>>;
  private readonly hubConnection: Promise<HubConnection>;
  private readonly displayedQuestionSubscription: Subscription;

  constructor(questionsStore: QuestionsStore) {
    this.allQuestions = questionsStore.questions;
    this.hubConnection = this.connect();
    this.displayedQuestion = from(this.hubConnection).pipe(
      flatMap(hc => this.getDisplayedQuestionStream(hc)),
      multicast(new ReplaySubject(1))
    );
    this.displayedQuestionSubscription = (this.displayedQuestion as ConnectableObservable<Question>).connect();
  }

  public async displayQuestion(questionId: string) {
    await this.invoke('DisplayQuestion', questionId);
  }

  public async hideQuestion() {
    await this.invoke('HideQuestion');
  }

  public ngOnDestroy(): void {
    this.dispose().catch((x) => console.error(x));
  }

  private getDisplayedQuestionStream(hubConnection: HubConnection) {
    const initialDisplayedQuestion = from(this.getCurrentDisplayedQuestionId());
    const currentDisplayedQuestionId = this.onDisplayedQuestionChanges(hubConnection).pipe(
      withLatestFrom(
        questionChange =>
          (questionChange.type === 'hidden' ? null : questionChange.newQuestionId)
      )
    );

    const displayedQuestionIdStream = concat(
      initialDisplayedQuestion,
      currentDisplayedQuestionId
    );

    return combineLatest(
      this.allQuestions,
      displayedQuestionIdStream
    ).pipe(
      map(([questions, displayedId]) => questions.get(displayedId))
    );
  }

  private async getCurrentDisplayedQuestionId() {
    return this.invoke<string>('GetDisplayedQuestionId');
  }

  private async invoke<T>(methodName: string, ...args: any[]) {
    const connection = await this.hubConnection;
    return connection.invoke<T>(methodName, ...args);
  }

  private async connect() {
    const url = '/displayed-question';
    const connection = new HubConnectionBuilder().withUrl(url).build();

    await connection.start();
    return connection;
  }

  private onDisplayedQuestionChanges(hubConnection: HubConnection) {
    return new Observable<DisplayedQuestionChange>(subscriber => {

      const onDisplayedQuestionChanged = (questionId: string) => {
        subscriber.next({ type: 'changed', newQuestionId: questionId });
      };

      const onQuestionHidden = () => {
        subscriber.next({ type: 'hidden' });
      };

      hubConnection.on('DisplayedQuestionChanged', onDisplayedQuestionChanged);
      hubConnection.on('DisplayedQuestionHidden', onQuestionHidden);

      hubConnection.onclose(error => {
        if (error) {
          subscriber.error(error);
        } else {
          subscriber.complete();
        }
      });

      return () => {
        hubConnection.off('DisplayedQuestionChanged', onDisplayedQuestionChanged);
        hubConnection.off('DisplayedQuestionHidden', onQuestionHidden);
      };
    });
  }

  private async dispose() {
    this.displayedQuestionSubscription.unsubscribe();
    const hubConnection = await this.hubConnection;
    await hubConnection.stop();
  }
}
