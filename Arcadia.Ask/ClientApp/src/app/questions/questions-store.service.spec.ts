import { TestBed } from '@angular/core/testing';

import { QuestionsStore } from './questions-store.service';

describe('QuestionsStoreService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: QuestionsStore = TestBed.get(QuestionsStore);
    expect(service).toBeTruthy();
  });
});
