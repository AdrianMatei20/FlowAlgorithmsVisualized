import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AhujaOrlinShortestPathComponent } from './ahuja-orlin-shortest-path.component';

describe('AhujaOrlinShortestPathComponent', () => {
  let component: AhujaOrlinShortestPathComponent;
  let fixture: ComponentFixture<AhujaOrlinShortestPathComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AhujaOrlinShortestPathComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AhujaOrlinShortestPathComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
