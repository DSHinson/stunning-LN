import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CategoryState } from './category.reducer';
import { CategoryDto } from '../../models/category.dto';
import { CategoryTreeDto } from '../../models/category-tree.dto';

/**
 * Feature selector to get the categories slice from the store
 */
export const selectCategoryFeature = createFeatureSelector<CategoryState>('categories');

/**
 * Select all flat categories
 */
export const selectAllCategories = createSelector(
  selectCategoryFeature,
  (state: CategoryState) => state.categories
);

/**
 * Select the hierarchical category tree
 */
export const selectCategoryTree = createSelector(
  selectCategoryFeature,
  (state: CategoryState) => state.tree
);

/**
 * Select loading state
 */
export const selectCategoryLoading = createSelector(
  selectCategoryFeature,
  (state: CategoryState) => state.loading
);

/**
 * Select error message if any
 */
export const selectCategoryError = createSelector(
  selectCategoryFeature,
  (state: CategoryState) => state.error
);
