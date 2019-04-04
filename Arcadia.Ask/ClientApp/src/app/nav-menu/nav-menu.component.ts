import { Component, ChangeDetectionStrategy } from '@angular/core';
import { ModeratorAuthorizationService } from '../identity/moderator/moderator-authorization.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavMenuComponent {
  constructor(public moderatorAuthService: ModeratorAuthorizationService) { }

  public signInAsModerator() {
    this.moderatorAuthService.signInAsModerator();
  }
}
