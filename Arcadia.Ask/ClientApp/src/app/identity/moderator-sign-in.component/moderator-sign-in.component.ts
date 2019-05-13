import { Component, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-moderator-sign-in',
  templateUrl: './moderator-sign-in.component.html',
  styleUrls: ['./moderator-sign-in.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModeratorLoginComponent {
  public model = {
    login: '',
    password: '',
  };
}
