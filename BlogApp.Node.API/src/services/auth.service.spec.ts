import { Test, TestingModule } from '@nestjs/testing';
import { AuthService } from '../services/auth.service';
import { HttpService } from '@nestjs/axios';

describe('AuthService', () => {
  let service: AuthService;
  let httpServiceMock = {
    get: () => {}
  };

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        AuthService,
        HttpService
      ],
    }).overrideProvider(HttpService).useValue(httpServiceMock)
    .compile();

    service = module.get<AuthService>(AuthService);
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });
});
