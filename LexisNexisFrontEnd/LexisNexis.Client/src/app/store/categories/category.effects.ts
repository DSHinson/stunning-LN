import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, switchMap, of } from 'rxjs';
import * as CategoryActions from './category.actions';
import { CATEGORY_SERVICE } from '../../services/categories/categories.token';
import { ICategoryService } from '../../services/categories/categories.interface';

/**
 * Handles side effects for categories, e.g., fetching from API.
 */
@Injectable()
export class CategoryEffects {
  /** Injected via token to avoid hard dependency */
  private readonly categoryService = inject<ICategoryService>(CATEGORY_SERVICE);

  /** Injected actions stream */
  private readonly actions$ = inject(Actions);

  /** Load all categories */
  loadCategories$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CategoryActions.loadCategories),
      switchMap(() =>
        this.categoryService.getCategories().pipe(
          map(categories => CategoryActions.loadCategoriesSuccess({ categories })),
          catchError(err =>
            of(
              CategoryActions.loadCategoriesFailure({
                error: err?.message ?? 'Failed to load categories'
              })
            )
          )
        )
      )
    )
  );

  /** Load category tree */
  loadCategoryTree$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CategoryActions.loadCategoryTree),
      switchMap(() =>
        this.categoryService.getCategoryTree().pipe(
          map(tree => CategoryActions.loadCategoryTreeSuccess({ tree })),
          catchError(err =>
            of(
              CategoryActions.loadCategoryTreeFailure({
                error: err?.message ?? 'Failed to load category tree'
              })
            )
          )
        )
      )
    )
  );
}
