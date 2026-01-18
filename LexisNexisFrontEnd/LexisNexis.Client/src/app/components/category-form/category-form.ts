import { Component, Input, Output, EventEmitter, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { CategoryDto } from '../../models/category.dto';
import * as CategoryActions from '../../store/categories/category.actions';
import { selectAllCategories } from '../../store/categories/category.selectors';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { CategoryState } from '../../store/categories/category.state';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms'; // <-- add this
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-category-form',
  imports: [CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatFormFieldModule,
    MatButtonModule,
    MatCardModule,
    MatSelectModule,
    MatIconModule],
  templateUrl: './category-form.html',
  styleUrl: './category-form.scss',
  standalone: true
})
export class CategoryForm {
  category?: CategoryDto;
  form!: FormGroup;
  categories$: Observable<CategoryDto[]>;
  categories: CategoryDto[] = [];

  constructor(private store: Store<CategoryState>, private fb: FormBuilder, private dialogRef: MatDialogRef<CategoryForm>, @Inject(MAT_DIALOG_DATA) data?: CategoryDto) {

    this.category = data;
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(250)]],
      parentCategoryId: [this.category?.parentCategoryId || null]
    });

    if (this.category) {
      this.form.patchValue(this.category);
    }

    this.categories$ = this.store.select(selectAllCategories);
    this.store.dispatch(CategoryActions.loadCategories());
    this.categories$.subscribe(cats => this.categories = cats);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const now = new Date().toISOString();

    const result: CategoryDto = {
      id: this.category?.id ?? 0,
      createdAt: this.category?.createdAt ?? now,
      updatedAt: now,
      ...this.form.value
    };

    this.dialogRef.close(result);
  }

  cancel(): void {
    this.dialogRef.close();
  }

}
