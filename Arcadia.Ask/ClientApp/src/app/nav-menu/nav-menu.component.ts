import { ChangeDetectionStrategy, Component } from '@angular/core';
import { PermissionService } from '../identity/permissions.service';
import { Observable } from 'rxjs';
import { SignInService } from '../identity/sign-in.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavMenuComponent {
  public isModerator$: Observable<boolean>;

  constructor(
    permissionsService: PermissionService,
    private readonly signInService: SignInService
  ) {
    this.isModerator$ = permissionsService.isUserModerator();
  }

  public signOut() {
    this.signInService.signOut().subscribe(() => window.location.reload());
  }
}
