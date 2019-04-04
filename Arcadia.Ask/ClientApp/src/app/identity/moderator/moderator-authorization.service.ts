import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ModeratorAuthorizationService {
  constructor(private readonly http: HttpClient) { }

  public signInAsModerator() {
    this.http.post('/auth/moderator/sign-in', '');
  }
}
