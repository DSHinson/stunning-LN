import { createReducer, on } from '@ngrx/store';
import * as CategoryActions from './category.actions';
import { CategoryDto } from '../../models/category.dto';
import { CategoryTreeDto } from '../../models/category-tree.dto';

/**
 * State interface for categories.
 * - `categories`: flat list of all categories
 * - `tree`: hierarchical category tree
 * - `loading`: whether any category API call is in progress
 * - `error`: error message if any API call fails
 */
export interface CategoryState {
  categories: CategoryDto[];
  tree: CategoryTreeDto[];
  loading: boolean;
  error?: string;
}

/**
 * Initial state for categories
 */
export const initialState: CategoryState = {
  categories: [],
  tree: [],
  loading: false,
  error: undefined
};

/**
 * Reducer function for categories
 */
export const categoryReducer = createReducer(
  initialState,

  // --- Load flat categories ---
  on(CategoryActions.loadCategories, (state) => ({
    ...state,
    loading: true,
    error: undefined
  })),
  on(CategoryActions.loadCategoriesSuccess, (state, { categories }) => ({
    ...state,
    categories,
    loading: false
  })),
  on(CategoryActions.loadCategoriesFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // --- Load category tree ---
  on(CategoryActions.loadCategoryTree, (state) => ({
    ...state,
    loading: true,
    error: undefined
  })),
  on(CategoryActions.loadCategoryTreeSuccess, (state, { tree }) => ({
    ...state,
    tree,
    loading: false
  })),
  on(CategoryActions.loadCategoryTreeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);
