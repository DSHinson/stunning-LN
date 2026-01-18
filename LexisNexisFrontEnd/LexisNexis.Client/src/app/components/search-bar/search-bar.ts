import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Store } from '@ngrx/store';
import { ProductState } from '../../store/products/product.state';
import * as ProductActions from '../../store/products/product.actions';
import { CategoryFilter } from '../category-filter/category-filter';

@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './search-bar.html',
  styleUrls: ['./search-bar.scss']
})
export class SearchBar {
  @Input() categoryFilter: CategoryFilter | null = null;
  @Input() selectedCategoryId: number | null = null;

  searchTerm: string = '';

  constructor(private store: Store<ProductState>) { }

  onSearch() {
    this.store.dispatch(
      ProductActions.loadProducts({
        search: this.searchTerm,
        category: this.selectedCategoryId ?? undefined
      })
    );
  }

  clearSearch() {
    if (this.categoryFilter) {
      this.categoryFilter.clearSelection();
    }

    this.searchTerm = '';
    this.selectedCategoryId = null;

    this.store.dispatch(ProductActions.setPage({ page: 1 }));
    this.store.dispatch(ProductActions.loadProducts({}));
  }
}
