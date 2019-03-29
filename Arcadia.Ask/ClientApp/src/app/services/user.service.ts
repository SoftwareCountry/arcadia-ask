import { Injectable } from '@angular/core';
import { Guid } from 'guid-typescript';

@Injectable()
export class UserService {
  public getCurrentUserGuid(): String {
    const guid = localStorage.getItem('guid');

    if (guid === null || guid === undefined || guid.length === 0) {
      const newGuid = Guid.create();
      localStorage.setItem('guid', newGuid.toString());

      return newGuid.toString();
    }

    return guid.toString();
  }
}
