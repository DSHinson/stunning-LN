import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, switchMap, of, concatMap, tap, mergeMap } from 'rxjs';
import * as CategoryActions from './category.actions';
import { CATEGORY_SERVICE } from '../../services/categories/categories.token';
import { ICategoryService } from '../../services/categories/categories.interface';
import { ToastService } from '../../services/toasts/toasts.service';
import { CategoryDto } from '../../models/category.dto';

/**
 * Handles side effects for categories, e.g., fetching from API.
 */
@Injectable()
export class CategoryEffects {
  private readonly categoryService = inject<ICategoryService>(CATEGORY_SERVICE);
  private readonly toastService = inject(ToastService);
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

  /**Save category */
  createCategory$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CategoryActions.createCategory),
      mergeMap(({ category }) =>
        this.categoryService.createCategory(category).pipe(
          map((newCategory: CategoryDto) =>
            CategoryActions.createCategorySuccess()
          ),
          catchError(error =>
            of(CategoryActions.createCategoryFailure({ error }))
          )
        )
      )
    )
  );


  /**Reload after save success */
  reloadAfterChange$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        CategoryActions.createCategorySuccess,
      ),
      concatMap(() => [
        CategoryActions.loadCategoryTree(),
        CategoryActions.loadCategories()
      ]),
      tap(() => {
        this.toastService.success('Saved Category successfully', 'Success');
      })
    )
  );

  createCategoryFailureToast$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(CategoryActions.createCategoryFailure),
        tap(({ error }) => {
          this.toastService.error(
            error?.error?.message ?? error?.message ?? 'Failed to create category'
          );
        })
      ),
    { dispatch: false }
  );

  /**delete category */
  deleteCategory$ = createEffect(() =>
    this.actions$.pipe(
      ofType(CategoryActions.deleteCategory),
      mergeMap(({ id }) =>
        this.categoryService.deleteCategory(id).pipe(
          map(() =>
            CategoryActions.deleteCategorySuccess()
          ),
          catchError(error =>
            of(CategoryActions.deleteCategoryFailure({ error }))
          )
        )
      )
    )
  );

    /**Reload after save success */
  reloadAfterDelete$ = createEffect(() =>
    this.actions$.pipe(
      ofType(
        CategoryActions.deleteCategorySuccess,
      ),
      concatMap(() => [
        CategoryActions.loadCategoryTree(),
        CategoryActions.loadCategories()
      ]),
      tap(() => {
        this.toastService.success('Deleted Category successfully', 'Success');
      })
    )
  );
}
