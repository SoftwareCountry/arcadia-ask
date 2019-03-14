import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { QuestionsStore } from '../questions/questions-store.service';
import { from } from 'rxjs';
import { map, flatMap } from 'rxjs/operators';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(questionsStore: QuestionsStore) {
    questionsStore.questions
      .subscribe(x => console.log(x.toArray()));
  }
}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
