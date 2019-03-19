import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatInputModule, MatFormFieldModule, MatButtonModule } from '@angular/material';
import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { AddQuestionFormComponent } from './add-question-form/add-question-form.component';
import { FormsModule } from '@angular/forms';
import { QuestionCardComponent } from './question-card/question-card.component';

@NgModule({
  declarations: [QuestionsListComponent, AddQuestionFormComponent, QuestionCardComponent],
  exports: [QuestionsListComponent, AddQuestionFormComponent],
  imports: [
    CommonModule,
    QuestionsRoutingModule,
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule
  ]
})
export class QuestionsModule { }
