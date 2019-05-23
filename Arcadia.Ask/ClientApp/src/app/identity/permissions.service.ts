import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Permissions } from './permissions';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  constructor(private readonly http: HttpClient) { }

  public getPermissions(): Observable<Permissions>{
    return this.http.get<Permissions>('/api/permissions');
  }

  public isUserModerator(): Observable<boolean> {
    return this.getPermissions().pipe(
      map(p =>
        p.canApproveQuestion &&
        p.canCreateQuestion &&
        p.canDeleteQuestion &&
        p.canDisplayQuestion &&
        p.canHideQuestion)
    );
  }
}
