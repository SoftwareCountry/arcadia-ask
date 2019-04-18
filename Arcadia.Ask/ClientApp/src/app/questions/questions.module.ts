import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatInputModule, MatFormFieldModule, MatButtonModule, MatListModule, MatIconModule, MatDialogModule } from '@angular/material';
import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { AddQuestionFormComponent } from './add-question-form/add-question-form.component';
import { FormsModule } from '@angular/forms';
import { QuestionCardComponent } from './question-card/question-card.component';
import { QuestionCreatedPopupComponent } from './add-question-form/question-created-popup';

@NgModule({
  declarations: [
    QuestionsListComponent,
    AddQuestionFormComponent,
    QuestionCardComponent,
    QuestionCreatedPopupComponent
  ],
  exports: [QuestionsListComponent, AddQuestionFormComponent],
  imports: [
    CommonModule,
    QuestionsRoutingModule,
    FormsModule,
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
