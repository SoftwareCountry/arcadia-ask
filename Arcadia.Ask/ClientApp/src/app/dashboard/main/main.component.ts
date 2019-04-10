import { Component } from '@angular/core';
import { PermissionService } from '../../identity/permissions.service';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent {
  public canEditQuestions: Observable<boolean>;

  public canVoteQuestions: Observable<boolean>;

  public canCreateQuestions: Observable<boolean>;

  constructor(permissionService: PermissionService) {
    const permissions = permissionService.getPermissions();
    this.canEditQuestions = permissions
      .pipe(map(p => p.canApproveQuestion && p.canCreateQuestion));

    this.canVoteQuestions = permissions.
      pipe(map(p => p.canVote));

    this.canCreateQuestions = permissions.
      pipe(map(p => p.canCreateQuestion));
  }
}
