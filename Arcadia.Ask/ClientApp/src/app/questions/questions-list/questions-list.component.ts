import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { Question } from '../question';

@Component({
  selector: 'app-questions-list',
  templateUrl: './questions-list.component.html',
  styleUrls: ['./questions-list.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class QuestionsListComponent implements OnInit {

  @Input()
  questions: Question[];

  constructor() { }

  ngOnInit() {
  }

}
