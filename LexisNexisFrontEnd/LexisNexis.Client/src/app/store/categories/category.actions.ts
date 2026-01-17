import { createAction, props } from '@ngrx/store';
import { CategoryDto } from '../../models/category.dto';
import { CategoryTreeDto } from '../../models/category-tree.dto';

/**
 * Actions for loading the flat list of categories
 */
export const loadCategories = createAction(
  '[Category] Load Categories'
);

export const loadCategoriesSuccess = createAction(
  '[Category] Load Categories Success',
  props<{ categories: CategoryDto[] }>()
);

export const loadCategoriesFailure = createAction(
  '[Category] Load Categories Failure',
  props<{ error: string }>()
);

/**
 * Actions for loading the tree structure of categories
 */
export const loadCategoryTree = createAction(
  '[Category] Load Category Tree'
);

export const loadCategoryTreeSuccess = createAction(
  '[Category] Load Category Tree Success',
  props<{ tree: CategoryTreeDto[] }>()
);

export const loadCategoryTreeFailure = createAction(
  '[Category] Load Category Tree Failure',
  props<{ error: string }>()
);
