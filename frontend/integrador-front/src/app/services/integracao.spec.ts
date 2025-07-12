import { TestBed } from '@angular/core/testing';

import { Integracao } from './integracao';

describe('Integracao', () => {
  let service: Integracao;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Integracao);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
