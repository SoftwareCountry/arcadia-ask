import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ModeratorLoginComponent } from './moderator-sign-in.component/moderator-sign-in.component';

const routes: Routes = [
  { path: 'sign-in/moderator', component: ModeratorLoginComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
