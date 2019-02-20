import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddQuestionRoutingModule } from './add-question-routing.module';
import { AddQuestionComponent } from './add-question/add-question.component';

@NgModule({
  declarations: [AddQuestionComponent],
  imports: [
    CommonModule,
    AddQuestionRoutingModule
  ]
})
export class AddQuestionModule { }
