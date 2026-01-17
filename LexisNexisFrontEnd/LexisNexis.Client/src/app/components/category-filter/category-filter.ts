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
  /** Observable of all categories */
  categories$: Observable<CategoryDto[]>;

  /** Current selection */
  selectedCategoryId: number | null = null;

  /** Emit selected category ID to parent */
  @Output() categorySelected = new EventEmitter<number | null>();

  constructor(private store: Store<CategoryState>) {
    this.categories$ = this.store.select(selectAllCategories);
    this.store.dispatch(CategoryActions.loadCategories());
  }

  /** Emit the selected category ID */
  onCategorySelected(categoryId: number | null) {
    this.selectedCategoryId = categoryId;
    this.categorySelected.emit(categoryId); // <-- emits to parent
  }
}
