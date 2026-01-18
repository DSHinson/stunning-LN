import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { CategoryDto } from '../../models/category.dto';
import { CategoryState } from '../../store/categories/category.reducer';
import { selectAllCategories, selectCategoryTree } from '../../store/categories/category.selectors';
import * as CategoryActions from '../../store/categories/category.actions';
import { CommonModule } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { CategoryFlat } from "../../components/category-flat/category-flat";
import { CategoryTree } from "../../components/category-tree/category-tree";
import { CategoryTreeDto } from '../../models/category-tree.dto';
import { CategoryForm } from '../../components/category-form/category-form';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-categories',
  imports: [MatListModule, CommonModule, MatTabsModule, MatIconModule, CategoryFlat, CategoryTree, MatButtonModule],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
  standalone: true
})
export class Categories {

  // Observable stream of flat categories from the store
  categories$: Observable<CategoryDto[]>;
  categoriesTree$: Observable<CategoryTreeDto[]>;

  constructor(private store: Store<CategoryState>, private dialog: MatDialog) {
    this.categories$ = this.store.select(selectAllCategories);
    this.categoriesTree$ = this.store.select(selectCategoryTree);

    this.store.dispatch(CategoryActions.loadCategories());
    this.store.dispatch(CategoryActions.loadCategoryTree());
  }

  openCreate(): void {
    this.dialog.open(CategoryForm, {
      width: '600px'
    }).afterClosed().subscribe((result: any) => {
      if (result) {
        // create category
        this.store.dispatch(CategoryActions.createCategory({ category: result }));
      }
    });
  }
}
