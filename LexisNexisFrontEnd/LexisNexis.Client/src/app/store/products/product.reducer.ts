import { createReducer, on } from '@ngrx/store';
import * as ProductActions from './product.actions';
import { initialProductState } from './product.state';

/**
 * Reducer defines how the ProductState changes
 * in response to dispatched actions.
 *
 * Must be a pure function.
 */
export const productReducer = createReducer(
  initialProductState,

  // Indicates that a load operation is in progress
  on(ProductActions.loadProducts, state => ({
    ...state,
    loading: true,
    error: undefined
  })),

  // Stores the retrieved products
  on(ProductActions.loadProductsSuccess, (state, { products }) => ({
    ...state,
    products,
    loading: false
  })),

  // Captures the error for UI feedback
  on(ProductActions.loadProductsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);
