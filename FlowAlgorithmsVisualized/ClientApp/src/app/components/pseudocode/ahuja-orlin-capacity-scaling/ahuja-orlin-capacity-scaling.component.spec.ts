import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AhujaOrlinCapacityScalingComponent } from './ahuja-orlin-capacity-scaling.component';

describe('AhujaOrlinCapacityScalingComponent', () => {
  let component: AhujaOrlinCapacityScalingComponent;
  let fixture: ComponentFixture<AhujaOrlinCapacityScalingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AhujaOrlinCapacityScalingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AhujaOrlinCapacityScalingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
