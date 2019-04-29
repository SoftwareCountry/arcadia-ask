import { Routes } from '@angular/router';

import { ModeratorLoginComponent } from './moderator-sign-in.component/moderator-sign-in.component';

export const IDENTITY_ROUTES: Routes = [
  { path: 'moderator', component: ModeratorLoginComponent, pathMatch: 'full' }
];
