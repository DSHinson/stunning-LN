import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';

import { ProductDto } from '../../models/product.dto';
import { Observable } from 'rxjs';
import { CategoryDto } from '../../models/category.dto';
import { CategoryState } from '../../store/categories/category.state';
import { Store } from '@ngrx/store';
import { selectAllCategories } from '../../store/categories/category.selectors';
import * as CategoryActions from '../../store/categories/category.actions';

@Component({
  selector: 'app-product-form',
  standalone: true,
  templateUrl: './product-form.html',
  styleUrl: './product-form.scss',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    MatSelectModule
  ]
})
export class ProductForm {
  form: FormGroup;
  product?: ProductDto;
  categories$: Observable<CategoryDto[]>;
  categories: CategoryDto[] = [];

  constructor(private store: Store<CategoryState>, private fb: FormBuilder, private dialogRef: MatDialogRef<ProductForm>, @Inject(MAT_DIALOG_DATA) data?: ProductDto) {
    this.product = data;

    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(250)]],
      sku: ['', [Validators.required, Validators.maxLength(50)]],
      categoryId: [null, Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      quantity: [0, [Validators.required, Validators.min(0)]]
    });

    if (this.product) {
      this.form.patchValue(this.product);
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

    const result: ProductDto = {
      id: this.product?.id ?? 0,
      createdAt: this.product?.createdAt ?? now,
      updatedAt: now,
      ...this.form.value
    };

    this.dialogRef.close(result);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
