import { CategoryFilter } from "../../components/category-filter/category-filter";
import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { ProductState } from '../../store/products/product.state';
import * as ProductActions from '../../store/products/product.actions';

import { Observable } from 'rxjs';
import { ProductDto } from '../../models/product.dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatDialog } from '@angular/material/dialog';
import { ProductForm } from '../../components/product-form/product-form';
import { MatIconModule } from '@angular/material/icon';

import { CategoryDto } from "../../models/category.dto";
import { selectAllCategories } from "../../store/categories/category.selectors";

import { ProductsList } from "../../components/products-list/products-list";
/**
 * Standalone component for displaying products.
 * It injects the NgRx store to read the product state
 * and dispatch actions if needed.
 */
@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatListModule,
    CategoryFilter,
    MatIconModule,
     ProductsList],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class Products {

  searchTerm: string = '';
  selectedCategoryId: number | null = null;
  categories$: Observable<CategoryDto[]>;
  categories: CategoryDto[] = [];


  constructor(private store: Store<ProductState>, private dialog: MatDialog) {
    this.categories$ = this.store.select(selectAllCategories);
    this.categories$.subscribe(cats => this.categories = cats);
  }

  onCategoryFilterChanged(categoryId: number | null) {
    this.selectedCategoryId = categoryId;
    this.store.dispatch(
      ProductActions.loadProducts({ search: this.searchTerm, category: this.selectedCategoryId ?? undefined })
    );
  }

  getCategoryName(id: number | undefined): string {
    if (id == null) return '';
    const category = this.categories.find(c => c.id === id);
    return category ? category.name : '';
  }


  clearSearch() {
    this.searchTerm = '';
    this.selectedCategoryId = null
    this.onSearch();
  }

  onSearch() {
    this.store.dispatch(
      ProductActions.loadProducts({ search: this.searchTerm, category: this.selectedCategoryId ?? undefined })
    );
  }

  openCreate(): void {
    this.dialog.open(ProductForm, {
      width: '600px'
    }).afterClosed().subscribe(result => {
      if (result) {
        // create product
        this.store.dispatch(ProductActions.createProduct({ product: result }));
      }
    });
  }
}
