import { CategoryFilter } from "../../components/category-filter/category-filter";
import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { ProductState } from '../../store/products/product.state';
import { selectAllProducts } from '../../store/products/product.selectors';
import * as ProductActions from '../../store/products/product.actions';
import { Observable } from 'rxjs';
import { ProductDto } from '../../models/product.dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';

/**
 * Standalone component for displaying products.
 * It injects the NgRx store to read the product state
 * and dispatch actions if needed.
 */
@Component({
  selector: 'app-products',
  standalone: true, // allows this component to be used without being declared in a module
  imports: [   CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatListModule,
    CategoryFilter], // required for common directives like *ngFor
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class Products {
  // Observable stream of products from the store
  products$: Observable<ProductDto[]>;

  searchTerm: string = '';
  selectedCategoryId: number | null = null;

  constructor(private store: Store<ProductState>) {
    this.products$ = this.store.select(selectAllProducts);
    // Initial load
    this.store.dispatch(ProductActions.loadProducts());
  }

  onCategoryFilterChanged(categoryId: number | null) {
    this.selectedCategoryId = categoryId;
  }

  onSearch() {
    this.store.dispatch(
      ProductActions.loadProducts()
    );
  }
}
