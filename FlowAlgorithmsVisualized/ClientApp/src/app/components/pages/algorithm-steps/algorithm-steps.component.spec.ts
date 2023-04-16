import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlgorithmStepsComponent } from './algorithm-steps.component';

describe('AlgorithmStepsComponent', () => {
  let component: AlgorithmStepsComponent;
  let fixture: ComponentFixture<AlgorithmStepsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AlgorithmStepsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlgorithmStepsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
