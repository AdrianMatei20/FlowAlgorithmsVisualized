import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EdmondsKarpComponent } from './edmonds-karp.component';

describe('EdmondsKarpComponent', () => {
  let component: EdmondsKarpComponent;
  let fixture: ComponentFixture<EdmondsKarpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EdmondsKarpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EdmondsKarpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
