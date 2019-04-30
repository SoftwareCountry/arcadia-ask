import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatGridListModule } from '@angular/material';

import { QuestionsModule } from '../questions/questions.module';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { MainComponent } from './main/main.component';

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
