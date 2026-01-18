import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryFlat } from './category-flat';

describe('CategoryFlat', () => {
  let component: CategoryFlat;
  let fixture: ComponentFixture<CategoryFlat>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryFlat]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryFlat);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
