import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule } from '@angular/forms';
import { MatButtonModule, MatFormFieldModule, MatIconModule, MatInputModule, MatListModule } from '@angular/material';

import { AddQuestionFormComponent } from './add-question-form/add-question-form.component';
import { DisplayedQuestionComponent } from './displayed-question/displayed-question.component';
import { QuestionCardComponent } from './question-card/question-card.component';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { QuestionsRoutingModule } from './questions-routing.module';

@NgModule({
  declarations: [QuestionsListComponent, AddQuestionFormComponent, QuestionCardComponent, DisplayedQuestionComponent],
  exports: [QuestionsListComponent, AddQuestionFormComponent, DisplayedQuestionComponent],
  imports: [
    CommonModule,
    QuestionsRoutingModule,
    FormsModule,
    FlexLayoutModule,
    MatIconModule,
    MatListModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ]
})
export class QuestionsModule { }
