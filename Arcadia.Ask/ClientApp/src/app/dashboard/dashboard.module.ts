import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { MainComponent } from './main/main.component';
import { QuestionsModule } from '../questions/questions.module';
import { MatGridListModule } from '@angular/material';

@NgModule({
  declarations: [MainComponent],
  imports: [
    MatGridListModule,
    CommonModule,
    DashboardRoutingModule,
    FlexLayoutModule,
    QuestionsModule
  ]
})
export class DashboardModule { }
