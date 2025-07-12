import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClonarTasksComponent } from './clonar-tasks-component';

describe('ClonarTasksComponent', () => {
  let component: ClonarTasksComponent;
  let fixture: ComponentFixture<ClonarTasksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClonarTasksComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClonarTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
