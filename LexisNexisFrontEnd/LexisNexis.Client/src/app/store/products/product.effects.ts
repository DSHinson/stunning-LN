import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, switchMap, of } from 'rxjs';
import * as ProductActions from './product.actions';
import { PRODUCT_SERVICE } from '../../services/products/product.token';
import { IProductService } from '../../services/products/product.interface';

/**
 * ProductEffects handles side effects for the product store.
 *
 * Effects are observable streams that react to dispatched actions
 * and can dispatch new actions, typically for async operations like HTTP requests.
 */
@Injectable()
export class ProductEffects {

  private readonly actions$ = inject(Actions);
  private readonly productService = inject<IProductService>(PRODUCT_SERVICE);

  /**
   * Effect that listens for the loadProducts action.
   * When dispatched, it calls the productService to fetch products,
   * then emits either a success or failure action.
   */
  loadProducts$ = createEffect(() =>
    this.actions$.pipe(
      // Listen only for loadProducts actions
      ofType(ProductActions.loadProducts),

      // switchMap cancels previous requests if a new action comes in
      switchMap(() =>
        this.productService.getProducts().pipe(
          // On success, dispatch loadProductsSuccess with the returned products
          map(products => ProductActions.loadProductsSuccess({ products })),

          // On error, dispatch loadProductsFailure with the error message
          catchError(err =>
            of(ProductActions.loadProductsFailure({
              error: err?.message ?? 'Failed to load products'
            }))
          )
        )
      )
    )
  );
}
