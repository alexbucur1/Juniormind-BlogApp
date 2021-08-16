import { TestBed } from '@angular/core/testing';

import { ValidationService } from './validation.service';

describe('ValidationService', () => {
  let service: ValidationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ValidationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('should return true for password with enough complexity', () => {
    expect(service.validatePassword("Password12+")).toBeTrue();
  });
  it('should return false for password without one symbol', () => {
    expect(service.validatePassword("Password12")).toBeFalse();
  });
  it('should return false for password without one digit', () => {
    expect(service.validatePassword("Password+--")).toBeFalse();
  });
  it('should return false for password without one upper case letter', () => {
    expect(service.validatePassword("password12+")).toBeFalse();
  });
  it('should return false for password without one lower case letter', () => {
    expect(service.validatePassword("PASSWORD1!2")).toBeFalse();
  });
  it('should return false for password with less than 8 chars', () => {
    expect(service.validatePassword("Pas1-")).toBeFalse();
  });
  it('should return true for valid email', () => {
    expect(service.validateEmail("alex.bucur98@yahoo.com")).toBeTrue();
  });
  it('should return false for email without "@"', () => {
    expect(service.validateEmail("alex.bucur98yahoo.com")).toBeFalse();
  });
  it('should return false for email without ".com"', () => {
    expect(service.validateEmail("alex.bucur@98yahoo.cm")).toBeFalse();
  });
});
