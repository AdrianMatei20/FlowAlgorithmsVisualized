import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FordFulkersonComponent } from './ford-fulkerson.component';

describe('FordFulkersonComponent', () => {
  let component: FordFulkersonComponent;
  let fixture: ComponentFixture<FordFulkersonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FordFulkersonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FordFulkersonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
