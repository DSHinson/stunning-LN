import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CategoryTreeDto } from '../../models/category-tree.dto';
import { CategoryDto } from '../../models/category.dto';
import * as CategoryActions from '../../store/categories/category.actions';
import { Store } from '@ngrx/store';
import { CategoryState } from '../../store/categories/category.state';

@Component({
  selector: 'app-category-tree',
  standalone: true,
  imports: [
    CommonModule,
    MatExpansionModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './category-tree.html',
  styleUrls: ['./category-tree.scss']
})
export class CategoryTree {
  @Input() categories: CategoryTreeDto[] = [];
  constructor(private store: Store<CategoryState>)
  {}

 deleteCategory(category: CategoryTreeDto) {
        // Optional: confirm deletion
        const confirmed = confirm(`Are you sure you want to delete "${category.name}"?`);
        if (!confirmed) return;

        this.store.dispatch(CategoryActions.deleteCategory({ id: category.id }));
  }
}
