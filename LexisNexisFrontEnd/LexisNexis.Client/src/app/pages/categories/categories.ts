import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { CategoryDto } from '../../models/category.dto';
import { CategoryState } from '../../store/categories/category.reducer';
import { selectAllCategories } from '../../store/categories/category.selectors';
import * as CategoryActions from '../../store/categories/category.actions';
import { CommonModule } from '@angular/common';
import { MatListModule, MatList } from '@angular/material/list';

@Component({
  selector: 'app-categories',
  imports: [MatList,MatListModule,CommonModule],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
  standalone: true
})
export class Categories {
  // Observable stream of flat categories from the store
  categories$: Observable<CategoryDto[]>;

  constructor(private store: Store<CategoryState>) {
    // Select the flat list from the store
    this.categories$ = this.store.select(selectAllCategories);

    // Dispatch action to load flat categories on component init
    this.store.dispatch(CategoryActions.loadCategories());
  }
}
