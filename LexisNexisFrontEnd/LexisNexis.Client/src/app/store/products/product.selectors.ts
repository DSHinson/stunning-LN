import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ProductState } from './product.state';

/**
 * Feature selector for the ProductState.
 * Key must match the store registration name.
 */
export const selectProductState =
  createFeatureSelector<ProductState>('products');

/**
 * Selects the full list of products.
 */
export const selectAllProducts = createSelector(
  selectProductState,
  state => state.products
);

/**
 * Selects loading state for UI feedback.
 */
export const selectProductsLoading = createSelector(
  selectProductState,
  state => state.loading
);

/**
 * Selects error state for error messaging.
 */
export const selectProductsError = createSelector(
  selectProductState,
  state => state.error
);
