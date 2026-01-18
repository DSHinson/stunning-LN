import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, switchMap, of, mergeMap } from 'rxjs';
import * as ProductActions from './product.actions';
import { PRODUCT_SERVICE } from '../../services/products/product.token';
import { IProductService } from '../../services/products/product.interface';
import { createProductFailure, createProductSuccess, deleteProduct, deleteProductFailure, deleteProductSuccess, updateProduct, updateProductFailure, updateProductSuccess } from './product.actions';
import { ProductDto } from '../../models/product.dto';

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
      switchMap(({page = 1, pageSize = 10, category, search }) =>
        this.productService.getProducts(page, pageSize, search, category).pipe(
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

  updateProduct$ = createEffect(() =>
    this.actions$.pipe(
      ofType(updateProduct),
      mergeMap(({ product }) =>
        this.productService.updateProduct(product.categoryId, product).pipe(
          map((updatedProduct: ProductDto) => updateProductSuccess({ product: updatedProduct })),
          catchError((error) => of(updateProductFailure({ error })))
        )
      )
    )
  );

    deleteProduct$ = createEffect(() =>
    this.actions$.pipe(
      ofType(deleteProduct),
      mergeMap(({ product }) =>
        this.productService.deleteProduct(product.id).pipe(
          map(() => deleteProductSuccess({ productId: product.id })),
          catchError((error) => of(deleteProductFailure({ error })))
        )
      )
    )
  );

    createProduct$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductActions.createProduct),
      mergeMap(({ product }) =>
        this.productService.createProduct(product).pipe(
          map((newProduct: ProductDto) => ProductActions.createProductSuccess({ product: newProduct })),
          catchError((error) => of(ProductActions.createProductFailure({ error })))
        )
      )
    )
  );
}
