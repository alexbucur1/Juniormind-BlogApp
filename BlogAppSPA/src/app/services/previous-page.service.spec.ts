import { TestBed } from '@angular/core/testing';

import { PreviousPageService } from './previous-page.service';
import { RouterTestingModule } from '@angular/router/testing';

describe('PreviousPageService', () => {
  let service: PreviousPageService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule
      ]
    });
    service = TestBed.inject(PreviousPageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
