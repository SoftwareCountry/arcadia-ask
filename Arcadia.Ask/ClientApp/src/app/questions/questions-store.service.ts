import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Question } from './question';
import { HubConnectionBuilder } from '@aspnet/signalr';
import { async } from '@angular/core/testing';

@Injectable({
  providedIn: 'root'
})
export class QuestionsStore {

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  }

  async getData() {
    const connection = new HubConnectionBuilder().withUrl('/questions').build();
    await connection.start();
    const data = await connection.invoke('GetQuestions');
    console.log(data);
    return data;
  }
}
