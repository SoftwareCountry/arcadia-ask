import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SignInModel } from './sign-in.model';

@Injectable({
  providedIn: 'root'
})
export class SignInService {
  constructor(private readonly http: HttpClient) {}

  public signInAsModerator(requestModel: SignInModel): Observable<any> {
    return this.http.post('/api/auth/moderator/sign-in', requestModel);
  }
}