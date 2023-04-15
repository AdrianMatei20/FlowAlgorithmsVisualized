import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenericWithAugmentingPathComponent } from './generic-with-augmenting-path.component';

describe('GenericWithAugmentingPathComponent', () => {
  let component: GenericWithAugmentingPathComponent;
  let fixture: ComponentFixture<GenericWithAugmentingPathComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenericWithAugmentingPathComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenericWithAugmentingPathComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
