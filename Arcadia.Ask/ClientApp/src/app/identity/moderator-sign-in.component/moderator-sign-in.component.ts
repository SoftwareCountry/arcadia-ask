import { Component, ChangeDetectionStrategy, ViewChild, ChangeDetectorRef } from '@angular/core';
import { SignInService } from '../sign-in.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

const STATUS_UNAUTHORIZED = 401;

@Component({
  selector: 'app-moderator-sign-in',
  templateUrl: './moderator-sign-in.component.html',
  styleUrls: ['./moderator-sign-in.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModeratorLoginComponent {
  public model = {
    login: '',
    password: '',
  };

  public invalidCredentials = false;

  constructor(private readonly signInService: SignInService, private readonly router: Router, private readonly cdr: ChangeDetectorRef) {}

  public submit() {
    this.signInService.signInAsModerator(this.model)
      .subscribe(
        () => this.router.navigate(['/']),
        (err: HttpErrorResponse) => {
          if (err.status  === STATUS_UNAUTHORIZED) {
            this.invalidCredentials = true;
            this.cdr.detectChanges();
          }
        }
      );
  }
}
