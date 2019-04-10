import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Permissions } from './permissions';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  constructor(private readonly http: HttpClient) { }

  public getPermissions(): Observable<Permissions>{
    return this.http.get<Permissions>('/api/permissions');
  }
}
