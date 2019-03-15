import { Component, OnInit, ChangeDetectionStrategy, Input, EventEmitter, Output } from '@angular/core';
import { Question } from '../question';

@Component({
  selector: 'app-question-card',
  templateUrl: './question-card.component.html',
  styleUrls: ['./question-card.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class QuestionCardComponent {

  @Input()
  public question: Question;

  @Output()
  public voted = new EventEmitter<boolean>();

  public didVote = false;

  constructor() { }

  public vote() {
    this.didVote = true;
    this.voted.emit(true);
  }

}
