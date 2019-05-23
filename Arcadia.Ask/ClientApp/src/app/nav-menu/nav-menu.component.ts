import { ChangeDetectionStrategy, Component } from '@angular/core';
import { PermissionService } from '../identity/permissions.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavMenuComponent {
  public isModerator$: Observable<boolean>;

  constructor(permissionsService: PermissionService) { 
    this.isModerator$ = permissionsService.getPermissions().pipe(
      map(p => p.canDeleteQuestion)
    );
  }
}
