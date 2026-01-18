import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { selectAllCategories } from '../../store/categories/category.selectors';
import { selectAllProducts, selectPagination, selectProductsLoading } from '../../store/products/product.selectors';
import { CategoryDto } from '../../models/category.dto';
import { map, Observable } from 'rxjs';
import { ProductDto } from '../../models/product.dto';
import { Store } from '@ngrx/store';
import { ProductState } from '../../store/products/product.state';
import * as CategoryActions from '../../store/categories/category.actions';
import * as ProductActions from '../../store/products/product.actions';
import { ProductForm } from '../product-form/product-form';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-products-list',
  imports: [MatProgressSpinnerModule,
    MatTableModule,
    CommonModule,
    MatIconModule,
    FormsModule,
    MatButtonModule
  ],
  templateUrl: './products-list.html',
  styleUrl: './products-list.scss',
  standalone: true,
})
export class ProductsList {
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;
  displayedColumns: string[] = [
    'id',
    'name',
    'description',
    'sku',
    'categoryId',
    'price',
    'quantity',
    'createdAt',
    'updatedAt',
    'edit'
  ];

  categories$: Observable<CategoryDto[]>;
  categories: CategoryDto[] = [];
  products$: Observable<ProductDto[]>;
  loading$: Observable<boolean>;
  pageFromState$: Observable<number>;

  constructor(private store: Store<ProductState>, private dialog: MatDialog) {
    this.products$ = this.store.select(selectAllProducts);
    this.loading$ = this.store.select(selectProductsLoading);
    this.categories$ = this.store.select(selectAllCategories);
    this.pageFromState$ = this.store.select(selectPagination).pipe(map(p => p.page));

    this.store.dispatch(ProductActions.loadProducts({ page: this.currentPage, pageSize: this.pageSize }));
    this.store.dispatch(CategoryActions.loadCategories());
    this.categories$.subscribe(cats => this.categories = cats);
    this.pageFromState$.subscribe(page => this.currentPage = page);
    this.store.dispatch(ProductActions.setPage({ page: 1 }));

  }

  goToPreviousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.store.dispatch(ProductActions.setPage({ page: this.currentPage }));
      this.store.dispatch(ProductActions.loadProducts({ page: this.currentPage, pageSize: this.pageSize }));
    }
  }

  goToNextPage() {

    this.currentPage++;
    this.store.dispatch(ProductActions.setPage({ page: this.currentPage }));
    this.store.dispatch(ProductActions.loadProducts({ page: this.currentPage, pageSize: this.pageSize }));

  }

  getCategoryName(id: number | undefined): string {
    if (id == null) return '';
    const category = this.categories.find(c => c.id === id);
    return category ? category.name : '';
  }

  editProduct(product: ProductDto) {
    const dialogRef = this.dialog.open(ProductForm, {
      width: '500px',
      data: product
    });

    dialogRef.afterClosed().subscribe((result: ProductDto | undefined) => {
      if (result) {
        this.store.dispatch(ProductActions.updateProduct({ product: result }));
      }
    });
  }

  deleteProduct(product: ProductDto) {
    // Optional: confirm deletion
    const confirmed = confirm(`Are you sure you want to delete "${product.name}"?`);
    if (!confirmed) return;

    this.store.dispatch(ProductActions.deleteProduct({ product: product }));
  }
}
