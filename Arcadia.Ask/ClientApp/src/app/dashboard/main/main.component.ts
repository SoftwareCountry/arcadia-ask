import { Component } from '@angular/core';
import { PermissionService } from 'src/app/identity/permissions.service';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent {
  public canEditQuestions: Observable<boolean>;

  constructor(permissionService: PermissionService) {
    this.canEditQuestions = permissionService.getPermissions()
      .pipe(map(p => p.canApproveQuestion && p.canCreateQuestion));
  }
}
