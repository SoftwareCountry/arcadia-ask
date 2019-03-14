import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { AddQuestionFormComponent } from './add-question-form/add-question-form.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [QuestionsListComponent, AddQuestionFormComponent],
  exports: [QuestionsListComponent, AddQuestionFormComponent],
  imports: [
    CommonModule,
    QuestionsRoutingModule,
    FormsModule
  ]
})
export class QuestionsModule { }
