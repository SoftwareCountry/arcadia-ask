import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { AddQuestionComponent } from './add-question/add-question.component';

@NgModule({
  declarations: [QuestionsListComponent, AddQuestionComponent],
  exports: [QuestionsListComponent, AddQuestionComponent],
  imports: [
    CommonModule,
    QuestionsRoutingModule
  ]
})
export class QuestionsModule { }
