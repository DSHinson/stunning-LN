import { createAction, props } from '@ngrx/store';
import { ProductDto } from '../../models/product.dto';

/**
 * Dispatched by the UI to request products from the backend.
 */
export const loadProducts = createAction(
  '[Product] Load Products',
    props<{
    page?: number;        // optional, defaults to 1 in API
    pageSize?: number;    // optional, default to 10 in API
    category?: number;    // optional category ID
    search?: string;      // optional search term
  }>()
);

/**
 * Dispatched when products are successfully retrieved.
 */
export const loadProductsSuccess = createAction(
  '[Product] Load Products Success',
  props<{ products: ProductDto[] }>()
);

/**
 * Dispatched when loading products fails.
 */
export const loadProductsFailure = createAction(
  '[Product] Load Products Failure',
  props<{ error: string }>()
);

// Action to update a product
export const updateProduct = createAction(
  '[Product] Update Product',
  props<{ product: ProductDto }>()
);

// Action dispatched on successful product update
export const updateProductSuccess = createAction(
  '[Product] Update Product Success',
  props<{ product: ProductDto }>()
);

// Action dispatched on failed product update
export const updateProductFailure = createAction(
  '[Product] Update Product Failure',
  props<{ error: any }>()
);

export const deleteProduct = createAction(
  '[Product] Delete Product',
  props<{ product: ProductDto }>()
);

export const deleteProductSuccess = createAction(
  '[Product] Delete Product Success',
  props<{ productId: number }>()
);

export const deleteProductFailure = createAction(
  '[Product] Delete Product Failure',
  props<{ error: any }>()
);

export const createProduct = createAction(
  '[Product] Create Product',
  props<{ product: ProductDto }>()
);

export const createProductSuccess = createAction(
  '[Product] Create Product Success',
  props<{ product: ProductDto }>()
);

export const createProductFailure = createAction(
  '[Product] Create Product Failure',
  props<{ error: any }>()
);
