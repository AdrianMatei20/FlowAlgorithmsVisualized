import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GabowComponent } from './gabow.component';

describe('GabowComponent', () => {
  let component: GabowComponent;
  let fixture: ComponentFixture<GabowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GabowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GabowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
