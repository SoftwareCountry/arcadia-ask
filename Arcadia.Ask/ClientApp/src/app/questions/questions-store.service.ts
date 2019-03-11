import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Question } from './question';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { async } from '@angular/core/testing';
import { Subject, Observable, from, merge } from 'rxjs';
import { flatMap, concat, scan, map, switchMap, startWith } from 'rxjs/operators';
import { Map } from 'immutable';

type QuestionChange = { type: 'added', question: Question } | { type: 'removed', id: string };

@Injectable({
  providedIn: 'root'
})
export class QuestionsStore {

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  }

  getData() {
    return from(this.connect()).pipe(
      flatMap(con => {
        const initialArray = from(this.getInitialArray(con));
        return initialArray.pipe(
          switchMap(seed => {
            return this.onQuestionChanges(con).pipe(
              scan((acc, value: QuestionChange) => this.applyQuestionsChange(acc, value), seed),
              startWith(seed)
            );
          })
        );
      })
    );
  }

  private applyQuestionsChange(acc: Map<string, Question>, change: QuestionChange): Map<string, Question> {
    if (change.type === 'added') {
      return acc.set(change.question.questionId, change.question);
    } else {
      return acc.remove(change.id);
    }
  }

  private async connect() {
    const connection = new HubConnectionBuilder().withUrl('/questions').build();
    await connection.start();
    return connection;
  }

  private async getInitialArray(hubConnection: HubConnection) {
    const data = await hubConnection.invoke<Question[]>('GetQuestions');
    const mappedData = data.map<[string, Question]>(x => [x.questionId, x]);
    return Map(mappedData);
  }

  private onQuestionChanges(hubConnection: HubConnection) {
    return new Observable<QuestionChange>(subscriber => {
      hubConnection.on('QuestionIsChanged', (question: Question) => {
        subscriber.next({ type: 'added', question });
      });

      hubConnection.on('QuestionIsRemoved', (id: string) => {
        subscriber.next({ type: 'removed', id });
      });

      hubConnection.onclose(error => {
        if (error) {
          subscriber.error(error);
        } else {
          subscriber.complete();
        }
      });
    });
  }

  private async getQuestions(hubConnection: HubConnection) {
    return hubConnection.invoke<Question>('GetQuestions');
  }

  private async createQuestion(hubConnection: HubConnection, text: string) {
    await hubConnection.invoke('CreateQuestion', text);
  }

  private async approveQuestion(hubConnection: HubConnection, questionId: string) {
    await hubConnection.invoke('ApproveQuestion', questionId);
  }

  private async upvoteQuestion(hubConnection: HubConnection, questionId: string) {
    await hubConnection.invoke('UpvoteQuestion', questionId);
  }

  private async downvoteQuestion(hubConnection: HubConnection, questionId: string) {
    await hubConnection.invoke('DownvoteQuestion', questionId);
  }

  private async removeQuestion(hubConnection: HubConnection, questionId: string) {
    await hubConnection.invoke('RemoveQuestion', questionId);
  }
}
