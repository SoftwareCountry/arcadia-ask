import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatInputModule, MatFormFieldModule, MatButtonModule, MatListModule, MatIconModule } from '@angular/material';
import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { AddQuestionFormComponent } from './add-question-form/add-question-form.component';
import { FormsModule } from '@angular/forms';
import { QuestionCardComponent } from './question-card/question-card.component';
import { DisplayedQuestionComponent } from './displayed-question/displayed-question.component';

@NgModule({
  declarations: [QuestionsListComponent, AddQuestionFormComponent, QuestionCardComponent, DisplayedQuestionComponent],
  exports: [QuestionsListComponent, AddQuestionFormComponent, DisplayedQuestionComponent],
  imports: [
    CommonModule,
    QuestionsRoutingModule,
    FormsModule,
    MatIconModule,
    MatListModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ]
})
export class QuestionsModule { }
