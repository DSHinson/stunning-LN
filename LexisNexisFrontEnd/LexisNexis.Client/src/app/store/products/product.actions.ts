import { createAction, props } from '@ngrx/store';
import { ProductDto } from '../../models/product.dto';

/**
 * Dispatched by the UI to request products from the backend.
 */
export const loadProducts = createAction(
  '[Product] Load Products'
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
