import { Injectable, OnDestroy } from '@angular/core';
import { Question, QuestionImpl } from './question';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { Observable, from, Subscription, ReplaySubject, ConnectableObservable } from 'rxjs';
import { flatMap, scan, switchMap, startWith, multicast } from 'rxjs/operators';
import { Map } from 'immutable';

type QuestionChange =
  { type: 'added', question: Question } |
  { type: 'removed', id: string } |
  { type: 'approved', id: string } |
  { type: 'votesChanged', id: string, votes: number };

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
    const connection = new HubConnectionBuilder().withUrl('/questions').build();
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
    const mappedData = data.map<[string, Question]>(x => [x.questionId, x]);
    return Map(mappedData);
  }

  private async invoke<T>(methodName: string, ...args: any[]) {
    const connection = await this.hubConnection;
    return connection.invoke<T>(methodName, ...args);
  }

  private onQuestionChanges(hubConnection: HubConnection) {
    return new Observable<QuestionChange>(subscriber => {

      const onQuestionChanged = (question: Question) => {
        subscriber.next({ type: 'added', question });
      };

      const onQuestionRemoved = (id: string) => {
        subscriber.next({ type: 'removed', id });
      };

      const onQuestionVotesChanged = (id: string, votes: number) => {
        subscriber.next({ type: 'votesChanged', id, votes });
      };

      const onQuestionApproved = (id: string) => {
        subscriber.next({ type: 'approved', id });
      };

      hubConnection.on('QuestionIsChanged', onQuestionChanged);
      hubConnection.on('QuestionVotesAreChanged', onQuestionVotesChanged);
      hubConnection.on('QuestionIsApproved', onQuestionApproved);
      hubConnection.on('QuestionIsRemoved', onQuestionRemoved);

      hubConnection.onclose(error => {
        if (error) {
          subscriber.error(error);
        } else {
          subscriber.complete();
        }
      });

      return () => {
        hubConnection.off('QuestionIsChanged', onQuestionChanged);
        hubConnection.off('QuestionVotesAreChanged', onQuestionVotesChanged);
        hubConnection.off('QuestionIsApproved', onQuestionApproved);
        hubConnection.off('QuestionIsRemoved', onQuestionRemoved);
      };
    });
  }

  private applyQuestionsChange(acc: Map<string, Question>, change: QuestionChange): Map<string, Question> {
    let updateAcc: Map<string, Question>;
    switch (change.type) {
      case 'added':
        updateAcc = acc.set(change.question.questionId, change.question);
        break;

      case 'removed':
        updateAcc = acc.remove(change.id);
        break;

      case 'votesChanged':
        const oldQuestion = acc.get(change.id);
        const questionWithUpdatedVotes = new QuestionImpl(
          oldQuestion.text,
          oldQuestion.author,
          change.votes,
          oldQuestion.isApproved,
          oldQuestion.didVote
        );
        updateAcc = acc.set(change.id, questionWithUpdatedVotes);
        break;

      case 'approved':
        const unapprovedQuestion = acc.get(change.id);
        const approvedQuestion = new QuestionImpl(
          unapprovedQuestion.text,
          unapprovedQuestion.author,
          unapprovedQuestion.votes,
          true,
          unapprovedQuestion.didVote
        );
        updateAcc = acc.set(change.id, approvedQuestion);
        break;
    }

    return updateAcc;
  }
}
