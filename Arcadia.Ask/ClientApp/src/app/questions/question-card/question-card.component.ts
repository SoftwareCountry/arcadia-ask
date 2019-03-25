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
  public editable: boolean;

  @Input()
  public question: Question;

  @Output()
  public voted = new EventEmitter<boolean>();

  @Output()
  public deleted = new EventEmitter<void>();

  @Output()
  public approved = new EventEmitter<void>();

  public didVote = false;

  public vote() {
    this.didVote = true;
    this.voted.emit(true);
  }

  public approve() {
    this.approved.emit();
  }

  public delete() {
    this.deleted.emit();
  }

}
