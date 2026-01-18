import { Component, EventEmitter, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { Observable } from 'rxjs';
import { CategoryDto } from '../../models/category.dto';
import { CategoryState } from '../../store/categories/category.state';
import { selectAllCategories } from '../../store/categories/category.selectors';
import * as CategoryActions from '../../store/categories/category.actions';

@Component({
  selector: 'app-category-filter',
  imports: [CommonModule, MatFormFieldModule, MatSelectModule],
  templateUrl: './category-filter.html',
  styleUrl: './category-filter.scss',
  standalone: true,
})
export class CategoryFilter {
  categories$: Observable<CategoryDto[]>;
  selectedCategoryId: number | null = null;
  @Output() categorySelected = new EventEmitter<number | null>();

  constructor(private store: Store<CategoryState>) {
    this.categories$ = this.store.select(selectAllCategories);
    this.store.dispatch(CategoryActions.loadCategories());
  }

  onCategorySelected(categoryId: number | null) {
    this.selectedCategoryId = categoryId;
    this.categorySelected.emit(categoryId);
  }

  clearSelection() {
    this.selectedCategoryId = null;
    this.categorySelected.emit(null);
  }
}
