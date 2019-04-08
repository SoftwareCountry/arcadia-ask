import { Component } from '@angular/core';
import { PermissionService } from 'src/app/identity/permissions.service';
import { Permissions } from 'src/app/identity/permissions';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent {
  private permissions: Permissions;

  constructor(permissionService: PermissionService) {
    permissionService.getPermissions().subscribe(data => this.permissions = data);
  }

  public canEditQuestions(): boolean {
    return this.permissions !== undefined &&
      this.permissions !== null &&
      this.permissions.canApproveQuestion &&
      this.permissions.canDeleteQuestion;
  }
}
