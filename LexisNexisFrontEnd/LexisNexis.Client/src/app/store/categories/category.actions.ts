import { createAction, props } from '@ngrx/store';
import { CategoryDto } from '../../models/category.dto';
import { CategoryTreeDto } from '../../models/category-tree.dto';


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

export const createCategory = createAction(
  '[Category] Save Category',
  props<{ category: CategoryDto }>()
);

export const createCategorySuccess = createAction(
  '[Category] Save Category Success'
);

export const createCategoryFailure = createAction(
  '[Category] Save Category Failure',
  props<{ error: any }>()
);

export const deleteCategory = createAction(
  '[Category] Delete Category',
  props<{ id: number }>()
);

export const deleteCategorySuccess = createAction(
  '[Category] Delete Category Success'
);

export const deleteCategoryFailure = createAction(
  '[Category] Delete Category Failure',
  props<{ error: any }>()
);
