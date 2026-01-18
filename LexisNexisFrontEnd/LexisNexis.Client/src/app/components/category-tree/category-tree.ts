import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CategoryTreeDto } from '../../models/category-tree.dto';

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
  @Input() selectedCategoryId: number | null = null;
  @Output() categorySelected = new EventEmitter<number>();
  @Output() categoryDeleted = new EventEmitter<number>();

  selectCategory(category: CategoryTreeDto) {
    this.categorySelected.emit(category.id);
  }
  deleteCategory(category: any) {
  if (confirm(`Are you sure you want to delete "${category.name}"?`)) {
    this.categoryDeleted.emit(category);
  }
}
}
