import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DinicComponent } from './dinic.component';

describe('DinicComponent', () => {
  let component: DinicComponent;
  let fixture: ComponentFixture<DinicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DinicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DinicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
