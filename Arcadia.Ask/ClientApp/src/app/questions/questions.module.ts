import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatInputModule, MatFormFieldModule, MatButtonModule, MatListModule, MatIconModule, MatDialogModule } from '@angular/material';
import { FormsModule } from '@angular/forms';

import { AddQuestionFormComponent } from './add-question-form/add-question-form.component';
import { DisplayedQuestionComponent } from './displayed-question/displayed-question.component';
import { QuestionCardComponent } from './question-card/question-card.component';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionCreatedPopupComponent } from './add-question-form/question-created-popup';

@NgModule({
  declarations: [
    QuestionsListComponent,
    AddQuestionFormComponent,
    QuestionCardComponent,
    QuestionCreatedPopupComponent,
    DisplayedQuestionComponent,
  ],
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
    MatInputModule,
    MatDialogModule,
  ],
  entryComponents: [QuestionCreatedPopupComponent]
})
export class QuestionsModule { }
