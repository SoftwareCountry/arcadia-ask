import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { map, share } from 'rxjs/operators';

import { PermissionService } from '../../identity/permissions.service';
import { QuestionsStore } from 'src/app/questions/questions-store.service';
import { DisplayedQuestionService } from 'src/app/questions/displayed-question.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
  providers: [QuestionsStore, DisplayedQuestionService]
})
export class MainComponent {
  public canEditQuestions: Observable<boolean>;

  public canVoteQuestions: Observable<boolean>;

  public canCreateQuestions: Observable<boolean>;

  public canHideQuestions: Observable<boolean>;

  public canDisplayQuestion: Observable<boolean>;

  constructor(permissionService: PermissionService) {
    const permissions = permissionService.getPermissions().pipe(share());
    this.canEditQuestions = permissions
      .pipe(map(p => p.canApproveQuestion && p.canDeleteQuestion));

    this.canVoteQuestions = permissions.
      pipe(map(p => p.canVote));

    this.canCreateQuestions = permissions.
      pipe(map(p => p.canCreateQuestion));

    this.canHideQuestions = permissions.
      pipe(map(p => p.canHideQuestion));

    this.canDisplayQuestion = permissions.
      pipe(map(p => p.canDisplayQuestion));
  }
}
