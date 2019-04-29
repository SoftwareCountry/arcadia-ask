import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatFormFieldModule, MatInputModule, MatButtonModule } from '@angular/material';
import { FormsModule } from '@angular/forms';

import { ModeratorLoginComponent } from './moderator-sign-in.component/moderator-sign-in.component';

@NgModule({
  declarations: [
    ModeratorLoginComponent
  ],
  imports: [
    FlexLayoutModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    FormsModule
  ]
})
export class IdentityModule { }
