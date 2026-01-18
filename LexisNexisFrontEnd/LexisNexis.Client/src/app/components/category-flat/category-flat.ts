import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryDto } from '../../models/category.dto';
import { Store } from '@ngrx/store';
import { MatDialog } from '@angular/material/dialog';
import { ProductState } from '../../store/products/product.state';
import { selectAllCategories } from '../../store/categories/category.selectors';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import * as CategoryActions from '../../store/categories/category.actions';

@Component({
  selector: 'app-category-flat',
  imports: [
    CommonModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './category-flat.html',
  styleUrl: './category-flat.scss',
  standalone: true
})
export class CategoryFlat {
  displayedColumns: string[] = [
    'id',
    'name',
    'description',
    'parentCategoryId',
    'actions'
  ];
  categories$: Observable<CategoryDto[]>;
  categories: CategoryDto[] = [];

  constructor(private store: Store<ProductState>, private dialog: MatDialog) {
    this.categories$ = this.store.select(selectAllCategories);
    this.categories$.subscribe(cats => this.categories = cats);
  }

  getCategoryName(id: number | undefined): string {
    if (id == null) return '';
    const category = this.categories.find(c => c.id === id);
    return category ? category.name : '';
  }

  deleteCategory(category: CategoryDto) {
        // Optional: confirm deletion
        const confirmed = confirm(`Are you sure you want to delete "${category.name}"?`);
        if (!confirmed) return;

        this.store.dispatch(CategoryActions.deleteCategory({ id: category.id }));
  }
}
