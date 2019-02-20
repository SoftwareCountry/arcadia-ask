import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-add-question',
  templateUrl: './add-question.component.html',
  styleUrls: ['./add-question.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddQuestionComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
