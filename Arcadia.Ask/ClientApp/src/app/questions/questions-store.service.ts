import { Injectable, OnDestroy } from '@angular/core';
import { Question, QuestionForSpecificUser, QuestionImpl } from './question';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { Observable, from, Subscription, ReplaySubject, ConnectableObservable } from 'rxjs';
import { flatMap, scan, switchMap, startWith, multicast } from 'rxjs/operators';
import { Map } from 'immutable';
import { UserService } from '../services/user.service';

type QuestionChange = { type: 'changed', question: Question } |
  { type: 'removed', id: string } |
  { type: 'voted', id: string };

@Injectable({
  providedIn: 'root'
})
export class QuestionsStore implements OnDestroy {
  public questions: Observable<Map<string, Question>>;

  private readonly hubConnection: Promise<HubConnection>;

  private readonly questionsSubscription: Subscription;

  constructor(private readonly userService: UserService) {
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
    const guidOfCurrentUser = this.userService.getCurrentUserGuid();
    const url = `/questions?guid=${guidOfCurrentUser}`;

    const connection = new HubConnectionBuilder().withUrl(url, {
    }).build();

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
    return this.invoke<QuestionForSpecificUser[]>('GetQuestions');
  }

  private async getInitialArray() {
    const data = await this.getQuestions();
    const mappedData = data.map<[string, Question]>(x => [
      x.question.questionId,
      new QuestionImpl(
        x.question.questionId,
        x.question.text,
        x.question.author,
        x.question.votes,
        x.question.isApproved,
        x.didVote,
      ),
    ]);
    return Map(mappedData);
  }

  private async invoke<T>(methodName: string, ...args: any[]) {
    const connection = await this.hubConnection;
    return connection.invoke<T>(methodName, ...args);
  }

  private onQuestionChanges(hubConnection: HubConnection) {
    return new Observable<QuestionChange>(subscriber => {

      const onQuestionChanged = (question: Question) => {
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
        return this.changeQuestion(acc, change.question);
      case 'removed':
        return acc.remove(change.id);
      case 'voted':
        const oldQuestion = acc.get(change.id);
        return acc.set(
          change.id,
          new QuestionImpl(
            oldQuestion.questionId,
            oldQuestion.text,
            oldQuestion.author,
            oldQuestion.votes,
            oldQuestion.isApproved,
            true
          )
        );
      default:
        console.error('Unknown change type');
    }
  }

  private changeQuestion(acc: Map<string, Question>, question: Question): Map<string, Question> {
    const oldQuestion = acc.get(question.questionId);
    const didVote = oldQuestion !== undefined &&
      oldQuestion !== null &&
      oldQuestion.didVote;

    return acc.set(
      question.questionId,
      new QuestionImpl(
        question.questionId,
        question.text,
        question.author,
        question.votes,
        question.isApproved,
        didVote
      )
    );
  }
}
