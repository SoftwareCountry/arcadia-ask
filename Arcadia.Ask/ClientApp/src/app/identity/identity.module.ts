import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatFormFieldModule, MatInputModule, MatButtonModule } from '@angular/material';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { ModeratorLoginComponent } from './moderator-sign-in.component/moderator-sign-in.component';
import { IdentityRoutingModule } from './identity-routing.module';

@NgModule({
  declarations: [
    ModeratorLoginComponent
  ],
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    IdentityRoutingModule,
    FormsModule
  ]
})
export class IdentityModule { }
